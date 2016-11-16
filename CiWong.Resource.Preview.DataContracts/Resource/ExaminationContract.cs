using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	[Serializable, DataContract(Name = "examination")]
	public class ExaminationContract : ResourceBase
	{
		public ExaminationContract()
		{
			this.Parts = Enumerable.Empty<QuestionModuleContract>();
		}

		/// <summary>
		/// 试卷标题
		/// </summary>
		[DataMember(Name = "title")]
		public string Title { get; set; }

		/// <summary>
		/// 试卷参考分值
		/// </summary>
		[DataMember(Name = "ref_score")]
		public float RefScore { get; set; }

		/// <summary>
		/// 试卷所属科目
		/// </summary>
		[DataMember(Name = "curriculum_id")]
		public int CurriculumID { get; set; }

		/// <summary>
		/// 试卷大题部分
		/// </summary>
		[DataMember(Name = "parts")]
		public IEnumerable<QuestionModuleContract> Parts { get; set; }
	}

	/// <summary>
	/// 大题部分
	/// </summary>
	[Serializable, DataContract(Name = "question_module")]
	public sealed class QuestionModuleContract
	{
		public QuestionModuleContract()
		{
			this.Children = Enumerable.Empty<QuestionContract>();
		}

		/// <summary>
		/// 排序编号
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

		/// <summary>
		/// 模块名称
		/// </summary>
		[DataMember(Name = "module_type_name")]
		public string ModuleTypeName { get; set; }

		/// <summary>
		/// 模块头部URL
		/// </summary>
		[DataMember(Name = "module_type_url")]
		public string ModuleTypeURL { get; set; }

		/// <summary>
		/// 子试题
		/// </summary>
		[DataMember(Name = "children")]
		public IEnumerable<QuestionContract> Children { get; set; }
	}
}
