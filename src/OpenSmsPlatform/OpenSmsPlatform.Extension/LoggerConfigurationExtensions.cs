using opensmsplatform.Common;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace opensmsplatform.Extension
{
    /// <summary>
    /// 日志配置扩展
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>
        public static LoggerConfiguration WriteToConsole(this LoggerConfiguration loggerConfiguration)
        {
            //输出普通日志
            loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
                lg
                .Filter.ByIncludingOnly(WithPropertyExt<string>(LogContextExtension.LogSource, s => !LogContextExtension.AopSql.Equals(s)))
                .WriteTo.Console());

            //输出SQL
            loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
                lg
                .Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextExtension.LogSource, s => LogContextExtension.AopSql.Equals(s)))
                .WriteTo.Console());

            return loggerConfiguration;
        }

        /// <summary>
        /// 输出到文件
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>

        public static LoggerConfiguration WriteToFile(this LoggerConfiguration loggerConfiguration)
        {
            //输出普通日志
            loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
                lg
                .Filter.ByIncludingOnly(WithPropertyExt<string>(LogContextExtension.LogSource, s => !LogContextExtension.AopSql.Equals(s)))
                .WriteTo.Async(s => s.File(Path.Combine("Logs", @"Log.txt"), rollingInterval: RollingInterval.Day,
                    outputTemplate: LogContextExtension.FileMessageTemplate, retainedFileCountLimit: 31)));
            //输出SQL
            loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
                lg
                .Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextExtension.LogSource, s => LogContextExtension.AopSql.Equals(s)))
                .WriteTo.Async(s => s.File(Path.Combine("Logs", LogContextExtension.AopSql, @"AopSql.txt"), rollingInterval: RollingInterval.Day,
                        outputTemplate: LogContextExtension.FileMessageTemplate, retainedFileCountLimit: 31)));

            //输出Api转发日志
            loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
                lg
                .Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextExtension.LogSource, s => LogContextExtension.ApiData.Equals(s)))
                .WriteTo.Async(s => s.File(Path.Combine("Logs", LogContextExtension.ApiData, @"ApiData.txt"), rollingInterval: RollingInterval.Day,
                        outputTemplate: LogContextExtension.FileMessageTemplate, retainedFileCountLimit: 31)));

            return loggerConfiguration;
        }

        public static Func<LogEvent, bool> WithPropertyExt<T>(string propertyName, Func<T, bool> predicate)
        {
            //如果不包含属性 也认为是true
            return e =>
            {
                if (!e.Properties.TryGetValue(propertyName, out var propertyValue)) return true;
                return propertyValue is ScalarValue { Value: T value } && predicate(value);
            };
        }
    }
}
