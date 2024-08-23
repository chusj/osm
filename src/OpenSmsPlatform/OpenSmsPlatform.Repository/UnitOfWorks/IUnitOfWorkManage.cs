using SqlSugar;
using System.Reflection;

namespace OpenSmsPlatform.Repository.UnitOfWorks
{
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();

        int TranCount { get; }

        /// <summary>
        /// 创建工作单元
        /// </summary>
        /// <returns></returns>
        UnitOfWork CreateUnitOfWork();

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        void BeginTran(MethodInfo method);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        void CommitTran(MethodInfo method);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();

        void RollbackTran(MethodInfo method);


    }
}
