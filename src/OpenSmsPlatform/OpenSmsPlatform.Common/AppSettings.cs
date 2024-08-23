using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace OpenSmsPlatform.Common
{
    /// <summary>
    /// appsetting.json操作类
    /// </summary>
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }
        public AppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AppSettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .Add(new JsonConfigurationSource
                {
                    Path = Path,
                    Optional = false,
                    ReloadOnChange = false
                })
                .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception)
            { }
            return string.Empty;
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }

        /// <summary>
        ///  根据路径 configuration["App:Name"];
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionPath)
        {
            try
            {
                return Configuration[sectionPath];
            }
            catch (Exception)
            { }
            return string.Empty;
        }
    }
}
