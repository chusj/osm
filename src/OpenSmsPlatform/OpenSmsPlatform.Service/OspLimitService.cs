using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository;
using SqlSugar;

namespace OpenSmsPlatform.Service
{
    public class OspLimitService : IOspLimitService
    {
        private readonly IBaseRepository<OspLimit> _limitRepository;

        public OspLimitService(IBaseRepository<OspLimit> limitRepository)
        {
            _limitRepository = limitRepository;
        }

        public async Task<OspLimit> IsInLimitList(string mobile)
        {
            var condition = Expressionable.Create<OspLimit>();
            condition.And(x => x.Mobile == mobile);

            List<OspLimit> limitList = await _limitRepository.Query(condition.ToExpression());
            if (limitList.Count() == 1)
            {
                return limitList[0];
            }
            else { return null; }
        }
    }
}
