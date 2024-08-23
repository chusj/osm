using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 账号充值
    ///</summary>
    [SugarTable("osp_acc_charge")]
    public class OspAccCharge
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsNullable = false, IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 账号id 
        ///</summary>
        [SugarColumn(ColumnName = "acc_id", IsNullable = false)]
        public string AccId { get; set; }

        /// <summary>
        /// 金额 
        ///</summary>
        [SugarColumn(ColumnName = "amount", IsNullable = false)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 充值前条数 
        ///</summary>
        [SugarColumn(ColumnName = "before_counts", IsNullable = false)]
        public int BeforeCounts { get; set; }

        /// <summary>
        /// 充值条数 
        ///</summary>
        [SugarColumn(ColumnName = "counts", IsNullable = false)]
        public int Counts { get; set; }

        /// <summary>
        /// 充值后条数 
        ///</summary>
        [SugarColumn(ColumnName = "after_counts", IsNullable = false)]
        public int AfterCounts { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remarks")]
        public string Remarks { get; set; }
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
