using OpenSmsPlatform.Model;
using SmsPackage.Model;
using SmsPackage.Service;

namespace OpenSmsPlatform.Api.Extensions
{
    public class SmsApi
    {
        private readonly IZhutongService _zhutongService;
        private readonly ILianluService _lianluService;

        public SmsApi(IZhutongService zhutongService,
            ILianluService lianluService)
        {
            _zhutongService = zhutongService;
            _lianluService = lianluService;
        }

        public async Task<ApiResponse> Send(string channel, List<string> mobiles, string content, string smsSuffix)
        {
            ApiResponse api = new ApiResponse();
            if (channel == "lianlu")
            {
                LianLuApiResponse response = await _lianluService.Send(mobiles, content, smsSuffix);
                if (response.Code == 200)
                {
                    api.Code = response.Code;
                }
                api.Message = response.Message;
            }
            else //if(channel == "zhutong")
            {
                ZhuTongApiResponse response = await _zhutongService.Send(mobiles, content);
                if(response.Code==200) 
                {
                    api.Code = response.Code;
                }
                api.Message = response.Message;
            }
            return api;
        }
    }
}
