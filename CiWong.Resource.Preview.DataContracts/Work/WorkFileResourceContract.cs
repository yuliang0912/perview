using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 作业附件资源
	/// </summary>
	[Serializable, DataContract(Name = "work_file_resource")]
	public class WorkFileResourceContract
	{
		[DataMember(Name = "user_id")]
		public long ContentId { get; set; }

		/// <summary>
		/// 打包ID
		/// </summary>
		[DataMember(Name = "record_id")]
		public long RecordId { get; set; }

		/// <summary>
		/// 附件名称
		/// </summary>
		[DataMember(Name = "file_name")]
		public string FileName { get; set; }

		/// <summary>
		/// 附件地址
		/// </summary>
		[DataMember(Name = "file_url")]
		public string FileUrl { get; set; }

		/// <summary>
		/// 附件格式(.png .mp4等)
		/// </summary>
		[DataMember(Name = "file_ext")]
		public string FileExt { get; set; }

		/// <summary>
		/// 文件类型(图片=1,音频=2,视频=3,Word文档=4)
		/// </summary>
		[DataMember(Name = "file_type")]
		public int FileType { get; set; }

		/// <summary>
		/// 文件描述
		/// </summary>
		[DataMember(Name = "file_desc")]
		public string FileDesc { get; set; }
	}
}
