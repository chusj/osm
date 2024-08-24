using SqlSugar;

namespace OpenSmsPlatform.Model
{
    /// <summary>
    /// 短信记录
    ///</summary>
    [SugarTable("osp_record")]
    public class OspRecord:RootEntityTkey<long>
    {
        /// <summary>
        /// 账号id 
        ///</summary>
        [SugarColumn(ColumnName = "acc_id", IsNullable = false)]
        public long AccId { get; set; }

        /// <summary>
        /// 手机号 
        ///</summary>
        [SugarColumn(ColumnName = "mobile", IsNullable = false)]
        public string Mobile { get; set; }
        /// <summary>
        /// 内容 
        ///</summary>
        [SugarColumn(ColumnName = "content", IsNullable = false)]
        public string Content { get; set; }

        /// <summary>
        /// 验证码 
        ///</summary>
        [SugarColumn(ColumnName = "code", IsNullable = false)]
        public string Code { get; set; }

        /// <summary>
        /// 是否为验证码 1.是 2.否 
        ///</summary>
        [SugarColumn(ColumnName = "is_code", IsNullable = false)]
        public int IsCode { get; set; }

        /// <summary>
        /// 验证码是否使用  1.是 2.否 
        ///</summary>
        [SugarColumn(ColumnName = "is_used", IsNullable = false)]
        public int IsUsed { get; set; }

        /// <summary>
        /// 发送时间 
        ///</summary>
        [SugarColumn(ColumnName = "send_on", IsNullable = false)]
        public DateTime SendOn { get; set; }

        /// <summary>
        /// 计费条数 
        ///</summary>
        [SugarColumn(ColumnName = "counts", IsNullable = false)]
        public int Counts { get; set; }

        /// <summary>
        /// 请求id 
        ///</summary>
        [SugarColumn(ColumnName = "request_id", IsNullable = false)]
        public string RequestId { get; set; }

        /// <summary>
        /// api编码 
        ///</summary>
        [SugarColumn(ColumnName = "api_code", IsNullable = false)]
        public string ApiCode { get; set; }

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
