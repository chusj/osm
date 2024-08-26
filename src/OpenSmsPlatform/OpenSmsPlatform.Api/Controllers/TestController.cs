using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenSmsPlatform.Common.Helper;
using OpenSmsPlatform.Model;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Hello World !";
        }

        [HttpPost]
        public string CreateRequestObj(string mobile)
        {
            SmsRequest smsRequest = new SmsRequest();
            smsRequest.Mobiles = new List<string> { mobile };
            smsRequest.Contents = "您的验证码：234442，为了您的信息安全，请不要转发验证码。【杭州希和】";
            smsRequest.Code = "234442";
            smsRequest.SmsSuffix = "【杭州希和】";
            smsRequest.AccId = "1724397308";
            smsRequest.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            smsRequest.RequestId = Guid.NewGuid().ToString();
            //签名
            smsRequest.Signature = CreateSignature(smsRequest);

            return JsonConvert.SerializeObject(smsRequest);
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <returns></returns>
        private string CreateSignature(SmsRequest request)
        {
            string AccKey = "1724397308415";
            string AccSecret = "152f540b511b44d58f4b68bf26d3435e";

            // 构建签名字符串
            string signString = $"AccId={request.AccId}" +
                                $"&AccKey={AccKey}" +
                                $"&AccSecret={AccSecret}" +
                                $"&SmfSuffix={request.SmsSuffix}" +
                                $"&TimeStamp={request.TimeStamp}";

            return Md5Helper.EncryptMD5(signString);
        }
    }
}
