using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Service;

namespace OpenSmsPlatform.Api.Controllers
{
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

        [HttpGet]
        public async Task<List<OspLimitVo>> GetLimit()
        {
            return await _limitService.Query(); ;
        }
    }
}
