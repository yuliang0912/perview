using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 单元作业详情
    /// </summary>
    [Serializable, DataContract(Name = "unit_works")]
    public class UnitWorksContract
    {
        public UnitWorksContract()
        {
            this.WorkAnswers = Enumerable.Empty<WorkAnswerContract>();
        }

        [DataMember(Name = "do_id")]
        public long DoId { get; set; }

        /// <summary>
        /// 布置资源内容ID
        /// </summary>
        [DataMember(Name = "content_id")]
        public long ContentId { get; set; }

        /// <summary>
        /// 布置记录ID
        /// </summary>
        [DataMember(Name = "record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// 作业ID(作业系统)
        /// </summary>
        [DataMember(Name = "work_id")]
        public long WorkId { get; set; }

        /// <summary>
        /// 做作业ID(作业系统)
        /// </summary>
        [DataMember(Name = "do_workid")]
        public long DoWorkId { get; set; }

        /// <summary>
        /// 提交人ID
        /// </summary>
        [DataMember(Name = "submit_user_id")]
        public int SubmitUserId { get; set; }

        /// <summary>
        /// 提交人姓名
        /// </summary>
        [DataMember(Name = "submit_user_name")]
        public string SubmitUserName { get; set; }

        /// <summary>
        /// 作业分值
        /// </summary>
        [DataMember(Name = "work_score")]
        public decimal WorkScore { get; set; }

        /// <summary>
        /// 实际得分
        /// </summary>
        [DataMember(Name = "actual_score")]
        public decimal ActualScore { get; set; }

        /// <summary>
        /// 正确率  
        /// </summary>
        [DataMember(Name = "correct_rate")]
        public decimal CorrectRate { get; set; }

        /// <summary>
        /// /作业时长
        /// </summary>
        [DataMember(Name = "work_long")]
        public int WorkLong { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [DataMember(Name = "submit_date")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// 是否超时
        /// </summary>
        [DataMember(Name = "is_timeout")]
        public bool IsTimeOut { get; set; }

        /// <summary>
        /// 作业提交次数
        /// </summary>
        [DataMember(Name = "submit_count")]
        public int SubmitCount { get; set; }

        /// <summary>
        /// 点评内容
        /// </summary>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// 点评类型(1:文本 2.语音)
        /// </summary>
        [DataMember(Name = "comment_type")]
        public int CommentType { get; set; }

        /// <summary>
        /// 0:未提交 1:暂存 2:已提交 3:已批改 4:退回
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 单元作业做题答案
        /// </summary>
        [DataMember(Name = "work_answers")]
        public IEnumerable<WorkAnswerContract> WorkAnswers { get; set; }
    }
}
