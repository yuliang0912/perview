using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 作业(自主练习)答案
    /// </summary>
    [Serializable, DataContract(Name = "work_answer")]
    public class WorkAnswerContract
    {
		/// <summary>
		/// 主键ID
		/// </summary>
		public long Id { get; set; }

        /// <summary>
        /// 学生作业ID或自主练习ID
        /// </summary>
        [DataMember(Name = "do_id")]
        public long DoId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember(Name = "submit_user_id")]
        public int SubmitUserId { get; set; }

        /// <summary>
        /// 资源版本ID
        /// </summary>
        [DataMember(Name = "version_id")]
        public long VersionId { get; set; }

        /// <summary>
        /// 答案类型(1:作业 2:自主练习)
        /// </summary>
        [DataMember(Name = "answer_type")]
        public int AnswerType { get; set; }

        /// <summary>
        /// 答案结果
        /// </summary>
        [DataMember(Name = "answers")]
        public string AnswerContent { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        [DataMember(Name = "score")]
        public decimal Score { get; set; }

        /// <summary>
        /// 1.正确 2.错误 3.半对 4:未知
        /// </summary>
        [DataMember(Name = "assess")]
        public int Assess { get; set; }
    }

    /// <summary>
    /// 作业(自主练习)答案
    /// </summary>
    [Serializable, DataContract(Name = "work_answer")]
    public class WorkAnswerContract<T> : WorkAnswerContract where T : IAnswer
    {
        /// <summary>
        /// 个性化答案实体
        /// </summary>
        [DataMember(Name = "answers")]
        public virtual List<T> Answers { get; set; }
    }
}
