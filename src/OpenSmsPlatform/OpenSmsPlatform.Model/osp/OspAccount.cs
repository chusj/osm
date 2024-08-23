using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 账号
    ///</summary>
    [SugarTable("osp_account")]
    public class OspAccount
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsNullable = false, IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        /// 账号名 
        ///</summary>
        [SugarColumn(ColumnName = "acc_name", IsNullable = false)]
        public string AccName { get; set; }
        /// <summary>
        /// 账号id 
        ///</summary>
        [SugarColumn(ColumnName = "acc_id", IsNullable = false)]
        public string AccId { get; set; }
        /// <summary>
        /// key 
        ///</summary>
        [SugarColumn(ColumnName = "acc_key", IsNullable = false)]
        public string AccKey { get; set; }
        /// <summary>
        /// secret 
        ///</summary>
        [SugarColumn(ColumnName = "acc_secret", IsNullable = false)]
        public string AccSecret { get; set; }
        /// <summary>
        /// 短信后缀 
        ///</summary>
        [SugarColumn(ColumnName = "sms_suffix", IsNullable = false)]
        public string SmsSuffix { get; set; }
        /// <summary>
        /// 短信余额 
        ///</summary>
        [SugarColumn(ColumnName = "acc_counts", IsNullable = false)]
        public int AccCounts { get; set; }
        /// <summary>
        /// 是否启用 1.是 2.否 
        ///</summary>
        [SugarColumn(ColumnName = "is_enable", IsNullable = false)]
        public byte IsEnable { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remarks")]
        public string Remarks { get; set; }
        /// <summary>
        /// api编码 
        ///</summary>
        [SugarColumn(ColumnName = "api_code")]
        public byte? ApiCode { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "create_on", IsNullable = false)]
        public DateTime CreateOn { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "create_uid", IsNullable = false)]
        public long CreateUid { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "create_by", IsNullable = false)]
        public string CreateBy { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "modify_on")]
        public DateTime? ModifyOn { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "modify_uid")]
        public long? ModifyUid { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "modify_by")]
        public string ModifyBy { get; set; }
    }
}
