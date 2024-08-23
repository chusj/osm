using AutoMapper;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository;
using SqlSugar;
using System.Linq.Expressions;

namespace OpenSmsPlatform.Service
{
    /// <summary>
    /// 基础服务实现
    /// </summary>
    /// <typeparam name="TEntity">泛型实体</typeparam>
    /// <typeparam name="TVo">泛型视图对象</typeparam>
    public class BaseService<TEntity, TVo> : IBaseService<TEntity, TVo> where TEntity : class, new()
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity> _baseRepository;

        public ISqlSugarClient Db => _baseRepository.Db;

        public BaseService(IMapper mapper, IBaseRepository<TEntity> baseRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            return await _baseRepository.Add(entity);
        }

        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await _baseRepository.Add(listEntity);
        }

        public async Task<int> AddSplit(TEntity entity)
        {
            return await _baseRepository.AddSplit(entity);
        }

        public async Task<int> AddSplit(List<TEntity> listEntity)
        {
            return await _baseRepository.AddSplit(listEntity);
        }

        public async Task<List<TVo>> Query()
        {
            //var baseRepo = new BaseRepository<TEntity>();
            //var entities = await baseRepo.Query();

            var entities = await _baseRepository.Query();
            //Console.WriteLine("_baseService 层 ：" + entities.GetHashCode());
            Console.WriteLine($"_baseRepository 实例HashCode ： {_baseRepository.GetHashCode()}");
            var llout = _mapper.Map<List<TVo>>(entities);
            return llout;

        }

        public async Task<List<TEntity>> Query(string where)
        {
            return await _baseRepository.Query(where);
        }

        public async Task<List<TEntity>> Query(string where, string orderByFields)
        {
            return await _baseRepository.Query(where, orderByFields);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _baseRepository.Query(whereExpression);
        }

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFileds)
        {
            return await _baseRepository.Query(whereExpression, top, orderByFileds);
        }

        /// <summary>
        /// 查询前N条数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string where, int top, string orderByFileds)
        {
            return await _baseRepository.Query(where, top, orderByFileds);
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await _baseRepository.QueryById(objId);
        }

        public async Task<PageModel<TEntity>> QueryPageSplit(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime,
            int pageIndex = 1, int pageSize = 20, string orderByFields = null)
        {
            return await _baseRepository.QueryPageSplit(whereExpression, beginTime, endTime,
                pageIndex, pageSize, orderByFields);
        }

        public async Task<bool> Delete(object objId)
        {
            return await _baseRepository.Delete(objId);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            return await _baseRepository.Delete(entity);
        }

        public async Task<bool> Delete(object[] objIds)
        {
            return await _baseRepository.Delete(objIds);
        }

        public async Task<bool> Update(TEntity entity)
        {
            return await _baseRepository.Update(entity);
        }
    }
}
