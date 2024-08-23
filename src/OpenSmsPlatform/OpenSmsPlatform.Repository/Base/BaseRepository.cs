using Microsoft.AspNetCore.Mvc.Filters;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository.UnitOfWorks;
using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenSmsPlatform.Repository
{
    /// <summary>
    /// 基础仓储实现
    /// </summary>
    /// <typeparam name="TEntity">泛型实体</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly SqlSugarScope _dbBase;
        public ISqlSugarClient Db => _db;
        private ISqlSugarClient _db
        {
            get
            {
                ISqlSugarClient db = _dbBase;

                var tenatAttr = typeof(TEntity).GetCustomAttribute<TenantAttribute>();
                if (tenatAttr != null)
                {
                    db = _dbBase.GetConnectionScope(tenatAttr.configId.ToString().ToLower());
                }

                return db;
            }
        }

        public BaseRepository(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _dbBase = unitOfWorkManage.GetDbClient();

        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var insert = _db.Insertable(entity);
            return await insert.ExecuteReturnEntityAsync();
        }

        public async Task<int> Add(List<TEntity> listEntity)
        {
            var insert = _db.Insertable(listEntity);
            return await insert.ExecuteCommandAsync();
        }

        public async Task<int> AddSplit(TEntity entity)
        {
            var insert = _db.Insertable(entity).SplitTable();
            return await insert.ExecuteCommandAsync();
        }

        public async Task<int> AddSplit(List<TEntity> listEntity)
        {
            var insert = _db.Insertable(listEntity).SplitTable();
            return await insert.ExecuteCommandAsync();
        }

        public async Task<bool> Delete(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Delete(object objId)
        {
            return await _db.Deleteable<TEntity>().In(objId).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Delete(object[] objIds)
        {
            return await _db.Deleteable<TEntity>().In(objIds).ExecuteCommandHasChangeAsync();
        }

        public async Task<List<TEntity>> Query()
        {
            await Console.Out.WriteLineAsync(Db.GetHashCode().ToString());
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> Query(string where)
        {
            return await _db.Queryable<TEntity>()
                .WhereIF(!string.IsNullOrEmpty(where), where)
                .ToListAsync();
        }

        public async Task<List<TEntity>> Query(string where, string orderByFields)
        {
            return await _db.Queryable<TEntity>()
                .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(!string.IsNullOrEmpty(where), where)
                .ToListAsync();
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>()
                .WhereIF(whereExpression != null, whereExpression)
                .ToListAsync();
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(whereExpression != null, whereExpression).Take(top).ToListAsync();
        }

        public async Task<List<TEntity>> Query(string where, int top, string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(!string.IsNullOrEmpty(where), where).Take(top).ToListAsync();
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await _db.Queryable<TEntity>().In(objId).SingleAsync();
        }

        public async Task<List<TEntity>> QueryById(object[] objIds)
        {
            return await _db.Queryable<TEntity>().In(objIds).ToListAsync();
        }

        public async Task<PageModel<TEntity>> QueryPageSplit(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime, int pageIndex = 1, int pageSize = 20, string orderByFields = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<TEntity>().SplitTable(beginTime, endTime)
                .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var data = new PageModel<TEntity>(pageIndex, totalCount, pageSize, list);
            return data;
        }


        public async Task<bool> Update(TEntity entity)
        {
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }
    }
}
