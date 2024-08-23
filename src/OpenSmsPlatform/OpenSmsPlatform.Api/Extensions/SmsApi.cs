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

        public static ApiResponse Send(List<string> mobiles, string content)
        {
            ApiResponse api = new ApiResponse();

            return api;
        }
    }
}
