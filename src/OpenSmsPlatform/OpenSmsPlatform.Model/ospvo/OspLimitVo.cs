namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 限制（黑/白名单）
    ///</summary>
    public class OspLimitVo
    {
        /// <summary>
        /// 添加类型 1.手动 2.自动 
        ///</summary>
        public int AddType { get; set; }

        /// <summary>
        /// 手机号码 
        ///</summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 限制类型 1.白名单 2.黑名单 
        ///</summary>
        public int LimitType { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        public string Remarks { get; set; }

        public DateTime CreateOn { get; set; }

        public long CreateUid { get; set; }

        public string CreateBy { get; set; }
    }
}
