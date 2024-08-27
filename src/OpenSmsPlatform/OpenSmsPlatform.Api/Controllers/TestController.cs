using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenSmsPlatform.Common.Helper;
using OpenSmsPlatform.Model;

namespace OpenSmsPlatform.Api.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 创建请求对象
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>返回发送短信时需要SmsRequest的JSON字符串</returns>
        [HttpPost]
        public string CreateRequestObj(string mobile)
        {
            SmsRequest smsRequest = new SmsRequest();
            smsRequest.Mobiles = new List<string> { mobile };
            smsRequest.Code = RandomCode().ToString();
            smsRequest.Contents = $"您的验证码：{ smsRequest.Code}，为了您的信息安全，请不要转发验证码。【杭州希和】";
            smsRequest.SmsSuffix = "【杭州希和】";
            smsRequest.AccId = "1724397308";
            smsRequest.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            smsRequest.RequestId = Guid.NewGuid().ToString();
            //签名
            smsRequest.Signature = CreateSignature(smsRequest);

            return JsonConvert.SerializeObject(smsRequest);
        }

        /// <summary>
        /// 随机验证码4位
        /// </summary>
        /// <returns></returns>
        private int RandomCode()
        {
            Random random = new Random();
            return random.Next(1000, 10000);
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <returns>返回签名</returns>
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
