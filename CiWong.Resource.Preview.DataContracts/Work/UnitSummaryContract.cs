using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 单元完成情况汇总
    /// </summary>
    [Serializable, DataContract(Name = "unit_summary")]
    public class UnitSummaryContract
    {
        /// <summary>
        /// 内容ID
        /// </summary>
        [DataMember(Name = "content_id")]
        public long ContentId { get; set; }

        /// <summary>
        /// 作业系统,作业ID
        /// </summary>
        [DataMember(Name = "work_id")]
        public long WorkId { get; set; }

        /// <summary>
        /// 布置记录ID
        /// </summary>
        [DataMember(Name = "record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// 布置人数
        /// </summary>
        [DataMember(Name = "total_num")]
        public int TotalNum { get; set; }

        /// <summary>
        /// 完成人数
        /// </summary>
        [DataMember(Name = "completed_num")]
        public int CompletedNum { get; set; }

        /// <summary>
        /// 批改人数
        /// </summary>
        [DataMember(Name = "mark_num")]
        public int MarkNum { get; set; }

        /// <summary>
        /// 点评人数
        /// </summary>
        [DataMember(Name = "comment_num")]
        public int CommentNum { get; set; }
    }
}
