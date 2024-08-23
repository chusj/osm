namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 账号充值视图模型
    ///</summary>
    public class OspAccChargeVo
    {
        public long Id { get; set; }

        /// <summary>
        /// 账号id 
        ///</summary>
        public string AccId { get; set; }

        /// <summary>
        /// 金额 
        ///</summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 充值前条数 
        ///</summary>
        public int BeforeCounts { get; set; }

        /// <summary>
        /// 充值条数 
        ///</summary>
        public int Counts { get; set; }

        /// <summary>
        /// 充值后条数 
        ///</summary>
        public int AfterCounts { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        public string Remarks { get; set; }

        public DateTime CreateOn { get; set; }

        public long CreateUid { get; set; }

        public string CreateBy { get; set; }

        public DateTime? ModifyOn { get; set; }

        public long? ModifyUid { get; set; }

        public string ModifyBy { get; set; }
    }
}
