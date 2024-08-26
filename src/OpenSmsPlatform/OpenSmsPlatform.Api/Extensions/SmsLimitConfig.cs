namespace OpenSmsPlatform.Api
{
    /// <summary>
    /// 短信限制配置
    /// </summary>
    public class SmsLimitConfig
    {
        /// <summary>
        /// 短信类型 1.验证码短信 2.普通短信
        /// </summary>
        public int SmsType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 每天最大条数
        /// </summary>
        public int DayMaxCount { get; set; }

        /// <summary>
        /// 每月最大条数
        /// </summary>
        public int MonthMaxCount { get; set; }
    }
}
