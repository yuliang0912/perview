using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 作业统计实体
	/// </summary>
	[Serializable, DataContract(Name = "work_census")]
	public class WorkCensusContract
	{
		/// <summary>
		/// 用户ID
		/// </summary>
		[DataMember(Name = "user_id")]
		public int UserId { get; set; }

		/// <summary>
		/// 用户名称
		/// </summary>
		[DataMember(Name = "user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// 总提交作业份数
		/// </summary>
		[DataMember(Name = "total_work_num")]
		public int TotalWorkNum { get; set; }

		/// <summary>
		/// 作业总提交次数
		/// </summary>
		[DataMember(Name = "total_submit_num")]
		public int TotalSubmitNum { get; set; }

		/// <summary>
		/// 作业总用时
		/// </summary>
		[DataMember(Name = "total_work_long")]
		public long TotalWorkLong { get; set; }
	}
}
