namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信商API
    ///</summary>
    public class OspApiVo
    {
        /// <summary>
        /// api通道编码 
        ///</summary>
        public string ApiCode { get; set; }

        /// <summary>
        /// 名称 
        ///</summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Api地址 
        ///</summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 是否启用 
        ///</summary>
        public byte IsEnabled { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        public string Remarks { get; set; }
        
        public string CreateOn { get; set; }
    }
}
