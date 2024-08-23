using Microsoft.AspNetCore.Http;
using Serilog;

namespace opensmsplatform.Common.Helper
{
    public class IpHelper
    {
        public static string GetIp(IHttpContextAccessor _httpContext)
        {
            try
            {
                var httpContext = _httpContext.HttpContext;
                if (httpContext == null)
                {
                    return ("HttpContext is null");
                }

                string ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                Log.Error("获取IP发生异常：" + ex.Message);
                return "throw exception";
            }
        }
    }
}
