namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// Api响应
    /// </summary>
    public class ApiResponse
    {
        public int Code { get; set; }  = 400;

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }
}
