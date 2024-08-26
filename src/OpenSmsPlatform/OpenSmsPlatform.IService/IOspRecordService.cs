using OpenSmsPlatform.Model;

namespace OpenSmsPlatform.IService
{
    public interface IOspRecordService
    {
        /// <summary>
        /// 添加记录并更新余额
        /// </summary>
        /// <param name="recordList">短信记录</param>
        /// <param name="account">账号实体</param>
        /// <returns></returns>
        Task<bool> AddRecordsAndUpdateAmount(List<OspRecord> recordList, OspAccount account);


        /// <summary>
        /// 查询一个月的记录
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="maxCount">最大条数</param>
        /// <param name="smsType">短信类型 1.验证码短信 2.普通短信</param>
        /// <returns></returns>
        Task<PageModel<OspRecord>> QueryMonthlyRecords(string mobile, DateTime endDate, int maxCount,int smsType);
    }
}
