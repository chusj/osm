using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 限制（黑/白名单）
    ///</summary>
    [SugarTable("osp_limit")]
    public class OspLimit
    {
        [SugarColumn(ColumnName = "id", IsNullable = false, IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 添加类型 1.手动 2.自动 
        ///</summary>
        [SugarColumn(ColumnName = "add_type", IsNullable = false)]
        public byte AddType { get; set; }

        /// <summary>
        /// 手机号码 
        ///</summary>
        [SugarColumn(ColumnName = "mobile", IsNullable = false)]
        public string Mobile { get; set; }

        /// <summary>
        /// 限制类型 1.白名单 2.黑名单 
        ///</summary>
        [SugarColumn(ColumnName = "limit_type", IsNullable = false)]
        public byte LimitType { get; set; }

        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remarks")]
        public string Remarks { get; set; }

        [SugarColumn(ColumnName = "create_on", IsNullable = false)]
        public DateTime CreateOn { get; set; }

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
