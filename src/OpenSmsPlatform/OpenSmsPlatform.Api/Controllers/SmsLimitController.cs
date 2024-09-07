using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Service;

namespace OpenSmsPlatform.Api.Controllers
{
    /// <summary>
    /// 短信限制控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmsLimitController : Controller
    {
        private readonly IBaseService<OspLimit, OspLimitVo> _limitService;
        private readonly IOspLimitService _ospLimitService;

        public SmsLimitController(IBaseService<OspLimit, OspLimitVo> limitService,
            IOspLimitService ospLimitService)
        {
            _limitService = limitService;
            _ospLimitService = ospLimitService;
        }

        /// <summary>
        /// 添加限制
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="limitType">限制类型：true 白名单的;false 黑名单</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OspLimit> AddLimit(string mobile, bool limitType, string remark)
        {
            OspLimit api = await _ospLimitService.IsInLimitList(mobile);
            if (api != null)
            {
                return api;
            }
            else
            {
                api = new OspLimit();
                api.AddType = 1;
                api.Mobile = mobile;
                api.LimitType = limitType ? 1 : 2;
                api.Remarks = remark;
                api.CreateOn = DateTime.Now;
                api.CreateBy = "admin";
                api.CreateUid = 0;

                return await _limitService.Add(api);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="limit">账号实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> EditLimit([FromBody] OspLimit limit)
        {
            OspLimit ospLimit = await _limitService.QueryById(limit.Id);
            if (ospLimit == null)
            {
                return new ApiResponse { Message = "要修改的纪录不存在" };
            }

            bool result = await _limitService.Update(limit);
            if (result)
            {
                return new ApiResponse { Code = 200, Message = "修改成功" };
            }
            else
            {
                return new ApiResponse { Message = "修改失败" };
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResponse> DeleteLimit(long id)
        {
            OspLimit ospLimit = await _limitService.QueryById(id);
            if (ospLimit == null)
            {
                return new ApiResponse { Message = "要删除的纪录不存在" };
            }


            bool result = await _limitService.Delete(id);
            if (result)
            {
                return new ApiResponse { Code = 200, Message = "删除成功" };
            }
            else
            {
                return new ApiResponse { Message = "删除失败" };
            }
        }

        [HttpGet]
        public async Task<List<OspLimitVo>> GetLimit()
        {
            return await _limitService.Query(); ;
        }
    }
}
