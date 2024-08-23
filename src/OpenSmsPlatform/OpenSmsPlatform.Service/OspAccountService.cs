using OpenSmsPlatform.Common;
using OpenSmsPlatform.Common.Helper;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Model;
using OpenSmsPlatform.Repository;
using SqlSugar;

namespace OpenSmsPlatform.Service
{
    public class OspAccountService : IOspAccountService
    {
        private readonly IBaseRepository<OspAccount> _accountRepository;

        public OspAccountService(IBaseRepository<OspAccount> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> ValidAccount(string accId, string timeStamp, string signature)
        {
            //1.计算时间戳差值
            long serverST = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long timeSpan = serverST - timeStamp.ObjToLong();

            if (timeSpan > 180 || timeSpan < -180)
            {
                return false;
            }

            //2.根据accid查询账号信息
            OspAccount account = await _accountRepository.QueryById(accId);
            if (account != null)
            {
                // 构建签名字符串
                string signString = $"AccId={accId}" +
                                    $"&AccKey={account.AccKey}" +
                                    $"&AccSecret={account.AccSecret}" +
                                    $"&SmfSuffix={account.SmsSuffix}" +
                                    $"&TimeStamp={timeStamp}";

                if (signature == Md5Helper.EncryptMD5(signString))
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<OspAccount> QueryOspAcount(string accId, string smsSuffix)
        {
            var condition = Expressionable.Create<OspAccount>();
            condition.And(x => x.AccId == accId && x.SmsSuffix == smsSuffix);

            List<OspAccount> accountList = await _accountRepository.Query(condition.ToExpression());
            if (accountList.Count() == 1)
            {
                return accountList[0];
            }
            else { return null; }
        }
    }
}
