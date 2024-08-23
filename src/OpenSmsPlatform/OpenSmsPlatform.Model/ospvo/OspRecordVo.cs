namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信记录
    ///</summary>
    public class OspRecordVo
    {
        public long Id { get; set; }

        /// <summary>
        /// 账号id 
        ///</summary>
        public long AccId { get; set; }

        /// <summary>
        /// 手机号 
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 内容 
        ///</summary>
        public string Content { get; set; }

        /// <summary>
        /// 验证码 
        ///</summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否为验证码 1.是 2.否 
        ///</summary>
        public byte IsCode { get; set; }

        /// <summary>
        /// 验证码是否使用  1.是 2.否 
        ///</summary>
        public byte IsUsed { get; set; }

        /// <summary>
        /// 发送时间 
        ///</summary>
        public DateTime SendOn { get; set; }

        /// <summary>
        /// 计费条数 
        ///</summary>
        public byte Counts { get; set; }

        /// <summary>
        /// 请求id 
        ///</summary>
        public string RequestId { get; set; }

        /// <summary>
        /// api编码 
        ///</summary>
        public byte ApiCode { get; set; }

        public DateTime CreateOn { get; set; }

        public long CreateUid { get; set; }

        public string CreateBy { get; set; }

        public DateTime? ModifyOn { get; set; }

        public long? ModifyUid { get; set; }

        public string ModifyBy { get; set; }
    }
}
