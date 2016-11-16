using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 维基试题答案存储格式实体
	/// </summary>
	[DataContract(Name = "question_answer")]
	public class QuestionAnswer : IAnswer
	{
		/// <summary>
		/// 答案位置下标
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

		/// <summary>
		/// 用户答案
		/// </summary>
		[DataMember(Name = "content")]
		public string Content { get; set; }

		/// <summary>
		/// 判定(1.正确 2.错误 3.半对 4.未知)
		/// </summary>
		[DataMember(Name = "assess")]
		public int Assess { get; set; }

		/// <summary>
		/// 选项得分
		/// </summary>
		[DataMember(Name = "item_score")]
		public decimal ItemScore { get; set; }
	}
}
