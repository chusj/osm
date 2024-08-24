using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenSmsPlatform.Common.Helper;
using OpenSmsPlatform.Model;
using Serilog;
using System.Security.Principal;
using System.Text;

namespace OpenSmsPlatform.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        public string Index()
        {
            return "Hello World !";
        }

        [HttpGet]
        public async Task<string> Send(string mobile, string url)
        {
            SmsRequest smsRequest = new SmsRequest();
            smsRequest.Mobiles = new List<string> { mobile };
            smsRequest.Contents = "";
            smsRequest.Code = "";
            smsRequest.SmsSuffix = "【杭州希和】";
            smsRequest.AccId = "12345";
            smsRequest.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            smsRequest.RequestId = Guid.NewGuid().ToString();
            //签名
            smsRequest.Signature = CreateSignature(smsRequest);


            (bool success, string message) resultTurple = await SendPostAsync(smsRequest, $"{url}/Sms/SendAsync");

            return $"发送结果：{resultTurple.success},返回消息：{resultTurple.message}";
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <returns></returns>
        private string CreateSignature(SmsRequest request)
        {
            string AccKey = "";
            string AccSecret = "";

            // 构建签名字符串
            string signString = $"AccId={request.AccId}" +
                                $"&AccKey={AccKey}" +
                                $"&AccSecret={AccSecret}" +
                                $"&SmfSuffix={request.SmsSuffix}" +
                                $"&TimeStamp={request.TimeStamp}";

            return Md5Helper.EncryptMD5(signString);
        }

        private async Task<(bool success, string message)> SendPostAsync(object request, string url)
        {
            (bool success, string message) resultTurple = (false, string.Empty);
            try
            {
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpClient _httpClient = new HttpClient();
                var response = await _httpClient.PostAsync(url, content);

                //返回
                resultTurple.message = await response.Content.ReadAsStringAsync();
                resultTurple.success = true;

                Log.Information($"External 请求原文：{jsonContent}");
                Log.Information($"External 接口：{url}");
                Log.Information($"External 返回：{resultTurple.message}");

                return resultTurple;
            }
            catch (HttpRequestException e)
            {
                resultTurple.message = $"SendPostAsync => Exception:{e.Message}";
                return resultTurple;
            }
        }

    }
}
