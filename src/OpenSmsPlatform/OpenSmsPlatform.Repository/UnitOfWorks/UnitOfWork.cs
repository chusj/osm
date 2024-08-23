using Microsoft.Extensions.Logging;
using SqlSugar;

namespace OpenSmsPlatform.Repository.UnitOfWorks
{
    /// <summary>
    /// 工作单元，用于控制事务自动提交、回滚
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        public ILogger Logger { get; set; }

        public ISqlSugarClient Db { get; internal set; }

        public ITenant Tenant { get; internal set; }

        public bool IsTran { get; internal set; }

        public bool IsCommit { get; internal set; }

        public bool IsClose { get; internal set; }


        public void Dispose()
        {
            if (IsTran && !IsCommit)
            {
                Logger.LogDebug("UnitOfWork RollbackTran");
                Tenant.RollbackTran();
            }

            //TODO:这个地方的Return是干啥的？
            if (Db.Ado.Transaction != null || IsClose)
                return;
            Db.Close();
        }

        public bool Commit()
        {
            if (IsTran && !IsCommit)
            {
                Logger.LogDebug("UnitOfWork CommitTran");
                Tenant.CommitTran();
                IsCommit = true;
            }

            if (Db.Ado.Transaction == null && !IsClose)
            {
                Db.Close();
                IsClose = true;
            }

            return IsCommit;
        }
    }
}
