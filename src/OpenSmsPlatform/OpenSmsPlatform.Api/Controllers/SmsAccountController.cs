using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using Serilog;
using System.Linq.Expressions;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmsAccountController : ControllerBase
    {
        private readonly IBaseService<OspAccount, OspAccountVo> _accountService;

        public SmsAccountController(IBaseService<OspAccount, OspAccountVo> accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<List<OspAccount>> GetAccount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            // 构建表达式
            Expression<Func<OspAccount, bool>> whereExpression = entity =>
                   entity.AccName.Contains(name);

            List<OspAccount> tempList = await _accountService.Query(whereExpression);

            //Todo => Swagger中返回id字段跟数据库中的实际值不等了。
            //Log.Information(JsonConvert.SerializeObject(tempList));

            return tempList;
        }

        [HttpPost]
        public async Task<OspAccount> AddAccount(string name, string smsSuffix,string remarks)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            OspAccount account = new OspAccount();
            account.AccName = name;
            account.SmsSuffix = string.IsNullOrEmpty(smsSuffix) ? $"【{name}】" : $"【{smsSuffix}】";
            account.Remarks = remarks;

            account.AccCounts = 0;
            account.IsEnable = 1;
            account.AccId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            account.AccKey = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            account.AccSecret = Guid.NewGuid().ToString("N");
            account.CreateOn = DateTime.Now;
            account.CreateBy = "admin";
            account.CreateUid = 0;
            account.ApiCode = "lianlu";

            return await _accountService.Add(account);
        }
    }
}
