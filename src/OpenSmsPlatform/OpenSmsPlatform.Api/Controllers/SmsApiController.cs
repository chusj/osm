using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmsApiController : ControllerBase
    {
        private readonly IBaseService<OspApi, OspApiVo> _apiService;

        public SmsApiController(IBaseService<OspApi, OspApiVo> apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        public async Task<OspApi> AddApi(string code,string name,string url,string remark) 
        {
            OspApi api = new OspApi();
            api.ApiCode = code;
            api.ApiName = name;
            api.ApiUrl = url;
            api.Remarks = remark;
            api.IsEnabled = 1;
            api.CreateOn = DateTime.Now;
            api.CreateBy = "admin";
            api.CreateUid = 0;

            return await _apiService.Add(api);
        }

        [HttpGet]
        public async Task<List<OspApiVo>> GetApi()
        {
            return await _apiService.Query(); ;
        }
    }
}
