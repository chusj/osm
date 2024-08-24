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
    }
}
