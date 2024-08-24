using OpenSmsPlatform.Common.Transcation;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository;

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
            int i = await _recordRepository.AddSplit(recordList);

            return await UpdateAccountAmount(account);
        }

        [UseTran(Propagation = Propagation.Required)]
        private async Task<bool> UpdateAccountAmount(OspAccount account)
        {
            return await _accountRepository.Update(account);
        }
    }
}
