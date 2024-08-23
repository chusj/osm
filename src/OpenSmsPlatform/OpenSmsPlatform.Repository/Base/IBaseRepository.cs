using opensmsplatform.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace opensmsplatform.Repository
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    /// <typeparam name="TEntity">泛型实体</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ISqlSugarClient Db { get; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="listEntity">多个实体</param>
        /// <returns></returns>
        Task<int> Add(List<TEntity> listEntity);

        /// <summary>
        /// 添加(自动分表)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<int> AddSplit(TEntity entity);

        /// <summary>
        /// 批量添加(自动分表)
        /// </summary>
        /// <param name="listEntity">多个实体</param>
        /// <returns></returns>
        Task<int> AddSplit(List<TEntity> listEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="objId">主键Id</param>
        /// <returns></returns>
        Task<bool> Delete(object objId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<bool> Delete(TEntity entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="objIds">多个主键Id</param>
        /// <returns></returns>
        Task<bool> Delete(object[] objIds);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <returns></returns>
        Task<List<TEntity>> Query(string where);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="where">where条件</param>
        /// <param name="orderByFields">排序字段 例如：name asc,create_on desc</param>
        /// <returns></returns>
        Task<List<TEntity>> Query(string where, string orderByFields);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereExpression">where表达式</param>
        /// <returns></returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// :查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFields);

        /// <summary>
        /// 查询前N条数据k
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> Query(string where, int top, string orderByFields);

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="objId">必须是主键，字段标记：[SugarColumn(IsPrimaryKey = true)]</param>
        /// <returns></returns>
        Task<TEntity> QueryById(object objId);

        /// <summary>
        /// 根据多个Id查询
        /// </summary>
        /// <param name="objIds"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryById(object[] objIds);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns></returns>
        Task<PageModel<TEntity>> QueryPageSplit(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime, int pageIndex = 1, int pageSize = 20, string orderByFields = null);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);
    }
}
