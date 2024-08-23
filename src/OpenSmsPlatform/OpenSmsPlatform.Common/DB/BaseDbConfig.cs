using SqlSugar;

namespace OpenSmsPlatform.Common.DB
{
    public class BaseDbConfig
    {
        /// <summary>
        /// 所有库配置
        /// </summary>
        public static readonly List<ConnectionConfig> AllConfigs = new();

        /// <summary>
        /// 有效配置连接（除Log库）
        /// </summary>
        public static List<ConnectionConfig> ValidConfig = new();

        public static ConnectionConfig MainConfig;

        public static ConnectionConfig LogConfig;

        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiConnectionString => MutiInitConn();

        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiInitConn()
        {
            List<MutiDBOperate> listdatabase = AppSettings.App<MutiDBOperate>("DBS")
                .Where(i => i.Enabled).ToList();
            var mainDbId = AppSettings.App(new string[] { "MainDB" }).ToString();
            var mainDbModel = listdatabase.Single(d => d.ConnId == mainDbId);
            listdatabase.Remove(mainDbModel);
            listdatabase.Insert(0, mainDbModel);

            foreach (var db in listdatabase)
            {
                SpecialDbString(db);
            }

            return (listdatabase, mainDbModel.Slaves);
        }

        /// <summary>
        /// 定制Db字符串
        /// 目的是保证安全：优先从本地txt文件获取，若没有文件则从appsettings.json中获取
        /// </summary>
        /// <param name="mutiDBOperate"></param>
        /// <returns></returns>
        private static MutiDBOperate SpecialDbString(MutiDBOperate mutiDBOperate)
        {
            if (mutiDBOperate.DbType == DataBaseType.Sqlite)
            {
                mutiDBOperate.Connection =
                    $"DataSource=" + Path.Combine(Environment.CurrentDirectory, mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.SqlServer)
            {
                mutiDBOperate.Connection = DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_SqlserverConn.txt",
                    mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.MySql)
            {
                mutiDBOperate.Connection =
                    DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_MySqlConn.txt", mutiDBOperate.Connection);
            }
            else if (mutiDBOperate.DbType == DataBaseType.Oracle)
            {
                mutiDBOperate.Connection =
                    DifDBConnOfSecurity(@"D:\my-file\dbCountPsw1_OracleConn.txt", mutiDBOperate.Connection);
            }

            return mutiDBOperate;
        }

        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (Exception)
                {
                }
            }

            return conn[conn.Length - 1];
        }
    }


    public class MutiDBOperate
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }

        /// <summary>
        /// 从库执行级别，数字越大，级别越高
        /// </summary>
        public int HitRate { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }

        /// <summary>
        /// 从库
        /// </summary>
        public List<MutiDBOperate> Slaves { get; set; }
    }

    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
    }
}
