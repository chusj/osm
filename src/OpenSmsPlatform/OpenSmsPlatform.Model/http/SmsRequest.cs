namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信请求对象
    /// </summary>
    public class SmsRequest
    {
        /// <summary>
		/// 手机号码
		/// </summary>
		public List<string> Mobiles { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        public string SmsSuffix { get; set; }

        /// <summary>
        /// 账号id
        /// </summary>
        public string AccId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 请求id（GUID）
        /// </summary>
        public string RequestId { get; set; }
    }
}
