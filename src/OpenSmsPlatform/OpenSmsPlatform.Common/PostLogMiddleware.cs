using Microsoft.AspNetCore.Http;
using Serilog;

namespace OpenSmsPlatform.Common
{
    public class PostLogMiddleware
    {
        private readonly RequestDelegate _next;

        public PostLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "POST")
            {
                using (LogContextExtension.Create.ApiDataPushProperty())
                {
                    var requestBody = await GetRequestBodyAsync(httpContext);
                    Log.Information($"Received POST request to {httpContext.Request.Path}. Body: {requestBody}");
                }
            }

            await _next(httpContext);
        }

        private static async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}
