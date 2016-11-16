using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 作业附件打包
	/// </summary>
	[Serializable, DataContract(Name = "work_resource")]
	public class WorkFilePackageContract
	{
		public WorkFilePackageContract()
		{
			this.WorkFileResources = Enumerable.Empty<WorkFileResourceContract>();
		}

		/// <summary>
		/// 打包ID
		/// </summary>
		[DataMember(Name = "record_id")]
		public long RecordId { get; set; }

		/// <summary>
		/// 附件资源包名称
		/// </summary>
		[DataMember(Name = "file_package_name")]
		public string FilePackageName { get; set; }

		/// <summary>
		/// 附件资源包类型(1:普通附件包 2:作业快传)
		/// </summary>
		[DataMember(Name = "file_package_type")]
		public int FilePackageType { get; set; }

		/// <summary>
		/// 创建人用户ID
		/// </summary>
		[DataMember(Name = "user_id")]
		public int UserId { get; set; }

		/// <summary>
		/// 创建人用户名称
		/// </summary>
		[DataMember(Name = "user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[DataMember(Name = "create_date")]
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 状态(0:正常 1:删除)
		/// </summary>
		[DataMember(Name = "status")]
		public int Status { get; set; }

		/// <summary>
		/// 作业附件资源集合
		/// </summary>
		[DataMember(Name = "work_file_resources")]
		public IEnumerable<WorkFileResourceContract> WorkFileResources { get; set; }
	}
}
