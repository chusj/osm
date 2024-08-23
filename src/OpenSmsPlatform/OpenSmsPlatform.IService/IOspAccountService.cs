using OpenSmsPlatform.Model;

namespace OpenSmsPlatform.IService
{
    public interface IOspAccountService
    {
        /// <summary>
        /// 查询账号
        /// </summary>
        /// <param name="accId">账号id</param>
        /// <param name="smsSuffix">短信签名</param>
        /// <returns></returns>
        Task<OspAccount> QueryOspAcount(string accId, string smsSuffix);

        /// <summary>
        /// 验证账号
        /// </summary>
        /// <param name="accId">账号id</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="signature">签名</param>
        /// <returns></returns>
        public Task<bool> ValidAccount(string accId, string timeStamp, string signature);
    }
}
