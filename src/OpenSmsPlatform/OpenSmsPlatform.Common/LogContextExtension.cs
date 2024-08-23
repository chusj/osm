using Serilog.Context;
using SqlSugar;

namespace opensmsplatform.Common
{
    /// <summary>
    /// 日志上下文拓展
    /// </summary>
    public class LogContextExtension : IDisposable
    {
        private readonly Stack<IDisposable> _disposableStack = new Stack<IDisposable>();

        public static LogContextExtension Create => new();

        /// <summary>
        /// 日志属性的Key值
        /// </summary>
        public static readonly string LogSource = "LogSource";

        /// <summary>
        /// Sql日志特性
        /// </summary>
        public static readonly string AopSql = "AopSql";

        /// <summary>
        /// Api数据
        /// </summary>
        public static readonly string ApiData = "ApiData";

        /// <summary>
        /// 日志信息模板
        /// </summary>
        public static readonly string FileMessageTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);

        public void AddStock(IDisposable disposable)
        {
            _disposableStack.Push(disposable);
        }

        public IDisposable SqlAopPushProperty(ISqlSugarClient db)
        {
            //对sqlsugar日志，打上一个属性
            AddStock(LogContext.PushProperty(LogSource, AopSql));

            return this;
        }

        public IDisposable ApiDataPushProperty()
        {
            AddStock(LogContext.PushProperty(LogSource, ApiData));

            return this;
        }


        public void Dispose()
        {
            while (_disposableStack.Count > 0)
            {
                _disposableStack.Pop().Dispose();
            }
        }
    }
}
