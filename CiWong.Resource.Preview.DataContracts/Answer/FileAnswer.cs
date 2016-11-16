using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 附件作业答案存放格式
	/// </summary>
	[DataContract(Name = "file_answer")]
	public class FileAnswer : IAnswer
	{
		/// <summary>
		/// 答案位置下标
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

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
		/// 点评
		/// </summary>
		[DataMember(Name = "comment")]
		public string Comment { get; set; }
	}
}
