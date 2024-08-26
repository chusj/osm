using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.Common;
using OpenSmsPlatform.Common.Helper;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository.UnitOfWorks;
using SmsPackage.Model;
using SmsPackage.Service;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly IOspAccountService _accountService;
        private readonly IOspRecordService _recordService;
        private readonly IOspLimitService _limitService;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IZhutongService _zhutongService;
        private readonly ILianluService _lianluService;

        public SmsController(IOspAccountService accountService,
            IOspRecordService recordService,
            IOspLimitService limitService,
            IUnitOfWorkManage unitOfWorkManage,
            IHttpContextAccessor httpContext,
            IConfiguration config)
        {
            _accountService = accountService;
            _recordService = recordService;
            _limitService = limitService;
            _unitOfWorkManage = unitOfWorkManage;
            _httpContext = httpContext;

            #region 短信api的配置项
            IServiceCollection services = new ServiceCollection();
            services.AddZhutongSendMessageApi(option =>
            {
                option.ApiUrl = config.GetValue<string>("ZhuTong:ApiUrl");
                option.ApiPath = config.GetValue<string>("ZhuTong:ApiPath");
                option.UserName = config.GetValue<string>("ZhuTong:UserName");
                option.Password = config.GetValue<string>("ZhuTong:Password");
            });
            services.AddLianluSendMessageApi(option =>
            {
                option.ApiUrl = config.GetValue<string>("LianLu:ApiUrl");
                option.ApiPath = config.GetValue<string>("LianLu:ApiPath");
                option.MchId = config.GetValue<string>("LianLu:MchId");
                option.AppId = config.GetValue<string>("LianLu:AppId");
                option.AppKey = config.GetValue<string>("LianLu:AppKey");
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            #endregion

            _zhutongService = serviceProvider.GetService<IZhutongService>();
            _lianluService = serviceProvider.GetService<ILianluService>();
        }

        public async Task<ApiResponse> SendAsync([FromBody] SmsRequest request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                //1.检查短信参数
                request.Mobiles = request.Mobiles.Where(p => p.Length == 11).ToList();  //过滤手机号
                var turple = CheckSmsParam(request);
                if (!turple.enable)
                {
                    response.Message = turple.msg;
                    return response;
                }

                //2.验证token
                if (!await _accountService.ValidAccount(request.AccId, request.TimeStamp, request.Signature))
                {
                    response.Code = 401;
                    response.Message = "Signature Error";
                    return response;
                }

                //3.(单发时)短信限制
                if (request.Mobiles.Count == 1)
                {
                    (bool enable, string msg) limitTruple = await CheckSendLimit(request.Mobiles[0], request.Code);
                    if (!limitTruple.enable)
                    {
                        response.Message = limitTruple.msg;
                        return response;
                    }
                }

                //4.检查账号
                int UseCounts = CalcUseCounts(request.Mobiles.Count, request.Contents.Length);
                (bool enable, string msg, OspAccount account) acountTuple = await CheckAccount(request, UseCounts);
                if (!acountTuple.enable)
                {
                    response.Message = acountTuple.msg;
                    return response;
                }
                OspAccount OspAccount = acountTuple.account;

                //5.发送短信
                if (OspAccount.ApiCode == "lianlu")
                {
                    LianLuApiResponse lianLuResponse = await _lianluService.Send(request.Mobiles, request.Contents, request.SmsSuffix);
                    response.Code = lianLuResponse.Code;
                    response.Message = lianLuResponse.Message;
                }
                else
                {
                    ZhuTongApiResponse zhutongResponse = await _zhutongService.Send(request.Mobiles, request.Contents);
                    response.Code = zhutongResponse.Code;
                    response.Message = zhutongResponse.Message;
                }

                if (response.Code == 200)
                {
                    //6. 扣费、保存记录(事务提交)
                    using var uow = _unitOfWorkManage.CreateUnitOfWork();
                    OspAccount.AccCounts = OspAccount.AccCounts - UseCounts;
                    bool flag = await _recordService.AddRecordsAndUpdateAmount(AppendList(request, OspAccount), OspAccount);

                    uow.Commit();

                    //7.最后返回
                    response.Message = response.Message ?? "发送成功";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = $"内部服务器错误: {ex.Message}";
                return response;
            }
        }

        /// <summary>
        /// 检查短信参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns>元组</returns>
        private (bool enable, string msg) CheckSmsParam(SmsRequest request)
        {
            (bool enable, string msg) checkTurple = (false, string.Empty);

            if (string.IsNullOrEmpty(request.AccId.Trim())
                || string.IsNullOrEmpty(request.Contents.Trim())
                || string.IsNullOrEmpty(request.SmsSuffix.Trim())
                || string.IsNullOrEmpty(request.Signature.Trim())
                || request.Mobiles.Count == 0)
            {
                checkTurple.msg = "参数缺失";
            }
            else if (!request.Contents.Contains(request.Code) && !string.IsNullOrEmpty(request.Code))
            {
                checkTurple.msg = "内容不包含验证码";
            }
            else if (!string.IsNullOrWhiteSpace(request.Contents) && !request.Contents.Contains(request.Signature))
            {
                checkTurple.msg = "内容缺少短信后缀";
            }
            else if (request.Mobiles.Count > 1000)
            {
                checkTurple.msg = "手机号码不可超过1000个";
            }
            else if (request.Contents.Length > 1000)
            {
                checkTurple.msg = "内容不可超过1000个字符";
            }
            else { checkTurple.enable = true; }

            return checkTurple;
        }

        /// <summary>
        /// 计算使用条数
        /// </summary>
        /// <param name="mobileCounts">手机号码数目</param>
        /// <param name="contentLength">内容长度</param>
        /// <returns></returns>
        private int CalcUseCounts(int mobileCounts, int contentLength)
        {
            int needCount = mobileCounts;              //小于70个字符，直接返回手机号码数
            if (contentLength > 70)
            {
                int SingleCount = contentLength / 67;  //先取商
                if ((contentLength % 67) > 0)          //再取余，有余数需要+1
                {
                    SingleCount = SingleCount + 1;
                }
                needCount = SingleCount * mobileCounts;
            }

            return needCount;
        }

        /// <summary>
        /// 检查账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="useCounts"></param>
        /// <returns></returns>
        private async Task<(bool enable, string msg, OspAccount account)> CheckAccount(SmsRequest request, int useCounts)
        {
            (bool enable, string msg, OspAccount account) checkTurple = (false, string.Empty, null);

            OspAccount smsAccount = await _accountService.QueryOspAcount(request.AccId, request.SmsSuffix);
            if (smsAccount == null)
            {
                checkTurple.msg = "签名错误";
            }
            if (smsAccount.IsEnable == 2)
            {
                checkTurple.msg = "账号停用";
            }
            else if (smsAccount.AccCounts < useCounts)
            {
                checkTurple.msg = "余额不足";
            }
            else
            {
                checkTurple.enable = true;
                checkTurple.account = smsAccount;
            }

            return checkTurple;
        }

        /// <summary>
        /// 拼接列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private List<OspRecord> AppendList(SmsRequest request, OspAccount account)
        {
            string ip = IpHelper.GetIp(_httpContext);
            List<OspRecord> list = new List<OspRecord>();
            foreach (var mobile in request.Mobiles)
            {
                OspRecord record = new OspRecord();
                record.AccId = account.Id;
                record.Mobile = mobile;
                record.Content = request.Contents;
                record.Code = request.Code;
                record.IsCode = string.IsNullOrEmpty(request.Code) ? 2 : 1;
                record.IsUsed = 2;
                record.SendOn = DateTime.Now;
                record.Counts = CalcUseCounts(request.Mobiles.Count, request.Contents.Length);
                record.RequestId = request.RequestId;
                record.ApiCode = account.ApiCode;
                record.CreateOn = DateTime.Now;
                record.CreateBy = "admin";
                record.CreateUid = 0;

                //record.Ip = ip;

                list.Add(record);
            }
            return list;
        }

        /// <summary>
        /// 检查发送显示
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        private async Task<(bool enable, string msg)> CheckSendLimit(string mobile, string code)
        {
            (bool enable, string msg) resultTurple = (true, string.Empty);
            int smsType = 1;
            if (string.IsNullOrEmpty(code))
            {
                smsType = 2;
            }

            //判断是否在限白名单中
            OspLimit ospLimit = await _limitService.IsInLimitList(mobile);
            if (ospLimit == null)
            {
                List<SmsLimit> list = AppSettings.App<SmsLimit>("SmsLimit");
                SmsLimit smsLimit = list.Where(x => x.SmsType == smsType).SingleOrDefault();
                if (smsLimit.Enabled)
                {
                    PageModel<OspRecord> page = await _recordService.QueryMonthlyRecords(mobile, DateTime.Now, smsLimit.MonthMaxCount, smsType);
                    if (smsLimit.MonthMaxCount >= page.dataCount)
                    {
                        resultTurple.enable = false;
                        resultTurple.msg = "达到当月发送最大值";
                    }
                    else if (smsLimit.DayMaxCount >= page.data.Count(x => x.CreateOn == DateTime.Today))
                    {
                        resultTurple.enable = false;
                        resultTurple.msg = "达到当日发送最大值";
                    }
                }
            }
            else if (ospLimit.LimitType == 2) //黑名单
            {
                resultTurple.enable = false;
                resultTurple.msg = "the phone number is on the blacklist";
            }

            return resultTurple;
        }
    }

    /// <summary>
    /// 短信限制
    /// </summary>
    public class SmsLimit
    {
        /// <summary>
        /// 短信类型 1.验证码短信 2.普通短信
        /// </summary>
        public int SmsType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 每天最大条数
        /// </summary>
        public int DayMaxCount { get; set; }

        /// <summary>
        /// 每月最大条数
        /// </summary>
        public int MonthMaxCount { get; set; }
    }
}
