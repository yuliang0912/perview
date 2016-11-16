using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 附件作业提交情况
	/// </summary>
	[Serializable, DataContract(Name = "file_works")]
	public class FileWorksContracts
	{
		public FileWorksContracts()
		{
			this.WorkAnswers = Enumerable.Empty<WorkAnswerContract>();
		}

		/// <summary>
		/// 主键ID
		/// </summary>
		[DataMember(Name = "do_id")]
		public long DoId { get; set; }

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
		[DataMember(Name = "work_level")]
		public decimal WorkLevel { get; set; }

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
		/// 作业附加留言
		/// </summary>
		[DataMember(Name = "message")]
		public string Message { get; set; }

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
		/// 用户提交的答案文件数量
		/// </summary>
		[DataMember(Name = "comment_type")]
		public int FileCount { get; set; }

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
