using System.Runtime.Serialization;

//听说训练答案实体类
namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 跟读作业答案单体
	/// </summary>
	[DataContract(Name = "read_answer")]
	public class ReadAnswerEntity : IAnswer
	{
		/// <summary>
		/// 内容序号
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

		/// <summary>
		/// 单词或者句子
		/// </summary>
		[DataMember(Name = "word")]
		public string Word { get; set; }

		/// <summary>
		/// 录音文件URL
		/// </summary>
		[DataMember(Name = "audio_url")]
		public string AudioUrl { get; set; }

		/// <summary>
		/// 跟读次数
		/// </summary>
		[DataMember(Name = "read_times")]
		public string ReadTimes { get; set; }
	}

	/// <summary>
	/// 听力模考试答案,一道题多个答案 保存一个LIST
	/// </summary>
	[DataContract(Name = "listen_answer")]
	public class ListenAnswerEntity : IAnswer
	{
		/// <summary>
		/// 内容序号
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

		/// <summary>
		/// 选项ID 先择题
		/// </summary>
		[DataMember(Name = "option_id")]
		public string OptionId { get; set; }

		/// <summary>
		/// 录音文件URL 情景对话录音文件
		/// </summary>
		[DataMember(Name = "audio_url")]
		public string AudioUrl { get; set; }

		/// <summary>
		/// 填空题
		/// </summary>
		[DataMember(Name = "blank_content")]
		public string BlankContent { get; set; }

	}
}
