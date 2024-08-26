using OpenSmsPlatform.Common.Transcation;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository;
using SqlSugar;

namespace OpenSmsPlatform.Service
{
    public class OspRecordService : IOspRecordService
    {
        private readonly IBaseRepository<OspAccount> _accountRepository;
        private readonly IBaseRepository<OspRecord> _recordRepository;

        public OspRecordService(
            IBaseRepository<OspAccount> accountRepository,
            IBaseRepository<OspRecord> recordRepository)
        {
            _accountRepository = accountRepository;
            _recordRepository = recordRepository;
        }

        [UseTran(Propagation = Propagation.Required)]
        public async Task<bool> AddRecordsAndUpdateAmount(List<OspRecord> recordList, OspAccount account)
        {
            await _recordRepository.AddSplit(recordList);

            return await UpdateAccountAmount(account);
        }

        /// <summary>
        /// 更新账号余额
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [UseTran(Propagation = Propagation.Required)]
        private async Task<bool> UpdateAccountAmount(OspAccount account)
        {
            return await _accountRepository.Update(account);
        }

        public async Task<PageModel<OspRecord>> QueryMonthlyRecords(string mobile,DateTime endDate,int maxCount,int smsType)
        {
            //由于分表保存，最大查询100条记录
            if(maxCount > 100)
            {
                maxCount = 100;
            }

            var condition = Expressionable.Create<OspRecord>();
            condition.And(exp: x => x.Mobile == mobile && x.IsCode== smsType);

            return await _recordRepository.QueryPageSplit(condition.ToExpression(), endDate.AddMonths(-1), endDate, 1,maxCount,"create_on desc");
        }
    }
}
