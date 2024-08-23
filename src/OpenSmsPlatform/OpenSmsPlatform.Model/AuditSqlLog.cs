using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opensmsplatform.Model
{
    [Tenant("log")]  //不区分大小写
    [SugarTable("AuditSqlLog_20231201", "Sql审计日志")]  //数据库表名，数据库表备注
    public class AuditSqlLog : BaseLog
    {

    }
}
