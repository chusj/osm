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

        /// <summary>
        /// 添加Api
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="url">url</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<OspApiVo>> GetApi()
        {
            return await _apiService.Query(); ;
        }
    }
}
