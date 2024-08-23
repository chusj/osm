namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 账号
    ///</summary>
    public class OspAccountVo
    {
        public long Id { get; set; }

        /// <summary>
        /// 账号名 
        ///</summary>
        public string AccName { get; set; }

        /// <summary>
        /// 账号id 
        ///</summary>
        public string AccId { get; set; }

        /// <summary>
        /// key 
        ///</summary>
        public string AccKey { get; set; }

        /// <summary>
        /// secret 
        ///</summary>
        public string AccSecret { get; set; }

        /// <summary>
        /// 短信后缀 
        ///</summary>
        public string SmsSuffix { get; set; }

        /// <summary>
        /// 短信余额 
        ///</summary>
        public int AccCounts { get; set; }

        /// <summary>
        /// 是否启用 1.是 2.否 
        ///</summary>
        public byte IsEnable { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        public string Remarks { get; set; }

        /// <summary>
        /// api编码 
        ///</summary>
        public byte? ApiCode { get; set; }

        public DateTime CreateOn { get; set; }

        public long CreateUid { get; set; }

        public string CreateBy { get; set; }

        public DateTime? ModifyOn { get; set; }

        public long? ModifyUid { get; set; }

        public string ModifyBy { get; set; }
    }
}
