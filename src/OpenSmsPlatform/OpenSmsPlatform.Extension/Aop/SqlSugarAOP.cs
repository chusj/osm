using opensmsplatform.Common;
using Serilog;
using SqlSugar;

namespace opensmsplatform.Extension
{
    public static class SqlSugarAOP
    {
        public static void OnLogExecuting(ISqlSugarClient sqlSugarScopeProvider, string user, string table, string operate, string sql, SugarParameter[] p, ConnectionConfig config)
        {
            try
            {
                var logConsole = string.Format($"------------------ \r\n User:[{user}]  Table:[{table}]  Operate:[{operate}] " +
                    $"ConnId:[{config.ConfigId}]【SQL语句】: " +
                    $"\r\n {UtilMethods.GetNativeSql(sql, p)}");

                //Console.WriteLine(logConsole);

                using (LogContextExtension.Create.SqlAopPushProperty(sqlSugarScopeProvider))
                {
                    //关闭sql日志输出到文件
                    Log.Information(logConsole);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured OnLogExcuting:" + e);
            }
        }
    }
}
