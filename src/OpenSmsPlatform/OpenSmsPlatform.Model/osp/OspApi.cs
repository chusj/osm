using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信商API
    ///</summary>
    [SugarTable("osp_api")]
    public class OspApi
    {
        [SugarColumn(ColumnName = "id", IsNullable = false, IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        /// api通道编码 
        ///</summary>
        [SugarColumn(ColumnName = "api_code", IsNullable = false)]
        public byte ApiCode { get; set; }

        /// <summary>
        /// 名称 
        ///</summary>
        [SugarColumn(ColumnName = "api_name", IsNullable = false)]
        public string ApiName { get; set; }

        /// <summary>
        /// Api地址 
        ///</summary>
        [SugarColumn(ColumnName = "api_url")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// 是否启用 
        ///</summary>
        [SugarColumn(ColumnName = "is_enabled", IsNullable = false)]
        public byte IsEnabled { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remarks")]
        public string Remarks { get; set; }
        
        [SugarColumn(ColumnName = "create_on", IsNullable = false)]
        public string CreateOn { get; set; }
        
        [SugarColumn(ColumnName = "create_uid", IsNullable = false)]
        public long CreateUid { get; set; }
        
        [SugarColumn(ColumnName = "create_by", IsNullable = false)]
        public string CreateBy { get; set; }
        
        [SugarColumn(ColumnName = "modify_on")]
        public DateTime? ModifyOn { get; set; }
        
        [SugarColumn(ColumnName = "modify_uid")]
        public long? ModifyUid { get; set; }
        
        [SugarColumn(ColumnName = "modify_by")]
        public string ModifyBy { get; set; }
    }
}
