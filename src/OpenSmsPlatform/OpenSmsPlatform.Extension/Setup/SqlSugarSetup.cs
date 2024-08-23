using Microsoft.Extensions.DependencyInjection;
using opensmsplatform.Common;
using opensmsplatform.Common.DB;
using SqlSugar;
using System.Text.RegularExpressions;

namespace opensmsplatform.Extension
{
    public static class SqlSugarSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (!string.IsNullOrEmpty(AppSettings.App("MainDb")))
            {
                MainDb.CurrentDbConnId = AppSettings.App("MainDb");
            }

            BaseDbConfig.MutiConnectionString.allDbs.ForEach(m =>
            {
                var config = new ConnectionConfig()
                {
                    ConfigId = m.ConnId.ObjToString().ToLower(),
                    ConnectionString = m.Connection,
                    DbType = (DbType)m.DbType,
                    IsAutoCloseConnection = true,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true,
                        SqlServerCodeFirstNvarchar = true,
                    },
                    InitKeyType = InitKeyType.Attribute
                };

                if (SqlSugarConst.LogConfigId.ToLower().Equals(m.ConnId.ToLower()))
                {
                    BaseDbConfig.LogConfig = config;
                }
                else
                {
                    BaseDbConfig.ValidConfig.Add(config);
                }

                BaseDbConfig.AllConfigs.Add(config);
            });

            if (BaseDbConfig.LogConfig == null)
            {
                throw new ApplicationException("未配置Log库连接");
            }

            /*
			 SqlSugarScope是线程安全，可使用单例注入
			 参考：https://www.donet5.com/Home/Doc?typeId=1181
			 */
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                return new SqlSugarScope(BaseDbConfig.AllConfigs, db =>
                {
                    BaseDbConfig.ValidConfig.ForEach(config =>
                    {
                        var dbProvider = db.GetConnectionScope((string)config.ConfigId);
                        // 配置实体数据权限（多租户）
                        RepositorySetting.SetTenantEntityFilter(dbProvider);

                        // 打印SQL语句
                        dbProvider.Aop.OnLogExecuting = (s, parameters) =>
                        {
                            SqlSugarAOP.OnLogExecuting(dbProvider, "xihe", ExtractTableName(s),
                                Enum.GetName(typeof(SugarActionType), dbProvider.SugarActionType), s, parameters,
                                config);
                        };
                    });
                });
            });
        }

        private static string ExtractTableName(string sql)
        {
            // 匹配 SQL 语句中的表名的正则表达式
            //string regexPattern = @"\s*(?:UPDATE|DELETE\s+FROM|SELECT\s+\*\s+FROM)\s+(\w+)";
            string regexPattern = @"(?i)(?:FROM|UPDATE|DELETE\s+FROM)\s+`(.+?)`";
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(sql);

            if (match.Success)
            {
                // 提取匹配到的表名
                return match.Groups[1].Value;
            }
            else
            {
                // 如果没有匹配到表名，则返回空字符串或者抛出异常等处理
                return string.Empty;
            }
        }
    }
}
