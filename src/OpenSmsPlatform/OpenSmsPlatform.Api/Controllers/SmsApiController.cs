using Microsoft.AspNetCore.Mvc;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using SqlSugar;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmsApiController : ControllerBase
    {
        private readonly IBaseService<OspApi, OspApiVo> _apiService;
        private readonly IBaseService<OspAccount, OspAccountVo> _accountService;

        public SmsApiController(IBaseService<OspApi, OspApiVo> apiService,
            IBaseService<OspAccount, OspAccountVo> accountService)
        {
            _apiService = apiService;
            _accountService = accountService;
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
        public async Task<OspApi> AddApi(string code, string name, string url, string remark)
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
        /// 修改
        /// </summary>
        /// <param name="api">api实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> EditAccount([FromBody] OspApi api)
        {
            OspApi ospApi = await _apiService.QueryById(api.Id);
            if (ospApi == null)
            {
                return new ApiResponse { Message = "要修改的纪录不存在" };
            }

            //apicode不允许修改
            api.ApiCode = ospApi.ApiCode;

            bool result = await _apiService.Update(api);
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
        public async Task<ApiResponse> DeleteAccount(long id)
        {
            OspApi ospApi = await _apiService.QueryById(id);
            if (ospApi == null)
            {
                return new ApiResponse { Message = "要删除的纪录不存在" };
            }

            //查询api有没有被账号使用
            var condition = Expressionable.Create<OspAccount>();
            condition.And(x => x.ApiCode == ospApi.ApiCode);
            List<OspAccount> accountList = await _accountService.Query(condition.ToExpression());
            if (accountList.Count() > 0)
            {
                return new ApiResponse { Message = "数据被使用无法删除" };
            }

            bool result = await _apiService.Delete(id);
            if (result)
            {
                return new ApiResponse { Code = 200, Message = "删除成功" };
            }
            else
            {
                return new ApiResponse { Message = "删除失败" };
            }
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
