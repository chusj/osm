using Microsoft.Extensions.Logging;
using OpenSmsPlatform.Common;
using SqlSugar;
using System.Collections.Concurrent;
using System.Reflection;

namespace OpenSmsPlatform.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<UnitOfWorkManage> _logger;

        private int _tranCount { get; set; }
        public int TranCount => _tranCount;

        public readonly ConcurrentStack<string> TranStack = new();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            _tranCount = 0;
        }


        public UnitOfWork CreateUnitOfWork()
        {
            UnitOfWork uow = new()
            {
                Logger = _logger,
                Db = _sqlSugarClient,
                Tenant = (ITenant)_sqlSugarClient,
                IsTran = true
            };

            uow.Db.Open();
            uow.Tenant.BeginTran();
            _logger.LogDebug("UnitOfWork Begin");
            return uow;
        }

        public SqlSugarScope GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarScope;
        }


        public void BeginTran()
        {
            lock (this)
            {
                _tranCount++;
                GetDbClient().BeginTran();
            }
        }

        public void BeginTran(MethodInfo method)
        {
            lock (this)
            {
                GetDbClient().BeginTran();
                TranStack.Push(method.GetFullName());
                _tranCount = TranStack.Count;
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void CommitTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }


                if (result == method.GetFullName())
                {
                    try
                    {
                        GetDbClient().CommitTran();

                        _logger.LogDebug($"Commit Transaction");
                        Console.WriteLine($"Commit Transaction");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                        _logger.LogDebug($"Commit Error , Rollback Transaction");
                    }
                    finally
                    {
                        while (!TranStack.TryPop(out _))
                        {
                            Thread.Sleep(1);
                        }

                        _tranCount = TranStack.Count;
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void RollbackTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }

                if (result == method.GetFullName())
                {
                    GetDbClient().RollbackTran();
                    _logger.LogDebug($"Rollback Transaction");
                    Console.WriteLine($"Rollback Transaction");
                    while (!TranStack.TryPop(out _))
                    {
                        Thread.Sleep(1);
                    }

                    _tranCount = TranStack.Count;
                }
            }
        }
    }
}
