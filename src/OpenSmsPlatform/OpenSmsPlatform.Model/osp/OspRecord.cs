using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信记录
    ///</summary>
    [SplitTable(SplitType.Month)] //按月分表 （自带分表支持 年、季、月、周、日）
    [SugarTable("osp_record_{year}{month}{day}")]
    //[SugarTable("osp_record")]
    public class OspRecord : RootEntityTkey<long>
    {
        /// <summary>
        /// 账号id 
        ///</summary>
        [SugarColumn(ColumnName = "acc_id", IsNullable = false)]
        public long AccId { get; set; }

        /// <summary>
        /// 手机号 
        ///</summary>
        [SugarColumn(ColumnName = "mobile", Length = 11, IsNullable = false)]
        public string Mobile { get; set; }

        /// <summary>
        /// 内容 
        ///</summary>
        [SugarColumn(ColumnName = "content", Length = 2000, IsNullable = false)]
        public string Content { get; set; }

        /// <summary>
        /// 验证码 
        ///</summary>
        [SugarColumn(ColumnName = "code", Length = 8, IsNullable = false)]
        public string Code { get; set; }

        /// <summary>
        /// 是否为验证码 1.是 2.否 
        ///</summary>
        [SugarColumn(ColumnName = "is_code", Length = 4, IsNullable = false)]
        public int IsCode { get; set; }

        /// <summary>
        /// 验证码是否使用  1.是 2.否 
        ///</summary>
        [SugarColumn(ColumnName = "is_used", Length = 4, IsNullable = false)]
        public int IsUsed { get; set; }

        /// <summary>
        /// 发送时间 
        ///</summary>
        [SplitField]
        [SugarColumn(ColumnName = "send_on", IsNullable = false)]
        public DateTime SendOn { get; set; }

        /// <summary>
        /// 计费条数 
        ///</summary>
        [SugarColumn(ColumnName = "counts", Length = 4, IsNullable = false)]
        public int Counts { get; set; }

        /// <summary>
        /// api编码 
        ///</summary>
        [SugarColumn(ColumnName = "api_code", Length = 20, IsNullable = false)]
        public string ApiCode { get; set; }

        /// <summary>
        /// 请求id 
        ///</summary>
        [SugarColumn(ColumnName = "request_id", Length = 40, IsNullable = false)]
        public string RequestId { get; set; }

        /// <summary>
        /// 请求IP
        ///</summary>
        [SugarColumn(ColumnName = "request_ip", Length = 40, IsNullable = true)]
        public string RequestIp { get; set; }

        [SugarColumn(ColumnName = "create_on", IsNullable = false)]
        public DateTime CreateOn { get; set; }

        [SugarColumn(ColumnName = "create_uid", IsNullable = false)]
        public long CreateUid { get; set; }

        [SugarColumn(ColumnName = "create_by", Length = 20, IsNullable = false)]
        public string CreateBy { get; set; }

        [SugarColumn(ColumnName = "modify_on", IsNullable = true)]
        public DateTime? ModifyOn { get; set; }

        [SugarColumn(ColumnName = "modify_uid", IsNullable = true)]
        public long? ModifyUid { get; set; }

        [SugarColumn(ColumnName = "modify_by", Length = 20, IsNullable = true)]
        public string ModifyBy { get; set; }
    }
}
