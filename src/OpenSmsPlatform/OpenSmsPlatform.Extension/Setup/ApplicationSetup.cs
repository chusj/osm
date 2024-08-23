using Microsoft.AspNetCore.Builder;
using opensmsplatform.Common.Core;
using Serilog;

namespace opensmsplatform.Extension
{
    public static class ApplicationSetup
    {
        public static void UseApplicationSetup(this WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                App.IsRun = true;
            });

            app.Lifetime.ApplicationStopped.Register(() =>
            {
                App.IsRun = false;

                //清除日志
                Log.CloseAndFlush();
            });
        }
    }
}
