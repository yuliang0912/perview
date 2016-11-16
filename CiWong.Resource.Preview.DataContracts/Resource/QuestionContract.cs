using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 试题资源
	/// </summary>
	[Serializable, DataContract(Name = "question")]
	public class QuestionContract : ResourceBase
	{
		public QuestionContract()
		{
			this.Opions = Enumerable.Empty<QuestionOption>();
			this.Attachments = Enumerable.Empty<Attachment>();
			this.KnowledgePoints = Enumerable.Empty<KnowledgePoint>();
			this.Children = Enumerable.Empty<QuestionContract>();
		}

		/// <summary>
		/// 排序编号
		/// </summary>
		[DataMember(Name = "sid")]
		public int Sid { get; set; }

		/// <summary>
		/// 题干
		/// </summary>
		[DataMember(Name = "stem")]
		public string Stem { get; set; }

		/// <summary>
		/// 科目ID
		/// </summary>
		[DataMember(Name = "curriculum_id")]
		public int CurriculumID { get; set; }

		/// <summary>
		/// 题目类型
		/// </summary>
		[DataMember(Name = "qtype")]
		public int Type { get; set; }

		/// <summary>
		/// 是否客观
		/// </summary>
		[DataMember(Name = "is_objective")]
		public bool IsObjective { get; set; }

		/// <summary>
		/// 父题版本ID
		/// </summary>
		[DataMember(Name = "parent_version")]
		public long ParentVersion { get; set; }

		/// <summary>
		/// 题目参考分值(兼容试卷模式)
		/// </summary>
		[DataMember(Name = "question_ref_sorce")]
		public float QuestionRefSorce { get; set; }

		/// <summary>
		/// 参考信息(答案.解题思路等)
		/// </summary>
		[DataMember(Name = "ref_info")]
		public QuestionRefInfo RefInfo { get; set; }

		/// <summary>
		/// 选项集合
		/// </summary>
		[DataMember(Name = "options")]
		public IEnumerable<QuestionOption> Opions { get; set; }

		/// <summary>
		/// 附件集合
		/// </summary>
		[DataMember(Name = "attachments")]
		public IEnumerable<Attachment> Attachments { get; set; }

		/// <summary>
		/// 知识点集合
		/// </summary>
		[DataMember(Name = "knowledge_points")]
		public IEnumerable<KnowledgePoint> KnowledgePoints { get; set; }

		/// <summary>
		/// 子题集合
		/// </summary>
		[DataMember(Name = "children")]
		public IEnumerable<QuestionContract> Children { get; set; }
	}

	/// <summary>
	/// 选项集合
	/// </summary>
	[Serializable, DataContract(Name = "question_option")]
	public class QuestionOption
	{
		public QuestionOption()
		{
			this.Attachments = Enumerable.Empty<Attachment>();
		}

		/// <summary>
		/// 选项Id
		/// </summary>
		[DataMember(Name = "id")]
		public long Id { get; set; }

		/// <summary>
		/// 选项题干
		/// </summary>
		[DataMember(Name = "stem")]
		public string Stem { get; set; }

		/// <summary>
		/// 附件集合
		/// </summary>
		[DataMember(Name = "attachments")]
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	/// <summary>
	/// 题目参考信息
	/// </summary>
	[Serializable, DataContract(Name = "question_ref_info")]
	public class QuestionRefInfo
	{
		public QuestionRefInfo()
		{
			this.Answers = new string[] { };
			this.Attachments = Enumerable.Empty<Attachment>();
		}

		/// <summary>
		/// 参考答案
		/// </summary>
		[DataMember(Name = "answers")]
		public string[] Answers { get; set; }

		/// <summary>
		/// 解题思路
		/// </summary>
		[DataMember(Name = "solving_idea")]
		public string SolvingIdea { get; set; }

		/// <summary>
		/// 解题思路附件
		/// </summary>
		[DataMember(Name = "attachments")]
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	/// <summary>
	/// 题目附件信息
	/// </summary>
	[Serializable, DataContract(Name = "question_attachment")]
	public class Attachment
	{
		/// <summary>
		/// 文件类型(Pic=1,Audio=2,Video=3,Doc=4)
		/// </summary>
		[DataMember(Name = "file_type")]
		public int FileType { get; set; }

		/// <summary>
		/// 文件位置(On=1,Up=2,Left=3,Right=4)
		/// </summary>
		[DataMember(Name = "position")]
		public int Position { get; set; }

		/// <summary>
		/// 文件地址
		/// </summary>
		[DataMember(Name = "file_url")]
		public string FileUrl { get; set; }
	}

	/// <summary>
	/// 题目知识点
	/// </summary>
	[Serializable, DataContract(Name = "question_attachment")]
	public class KnowledgePoint
	{
		/// <summary>
		/// 知识点ID
		/// </summary>
		[DataMember(Name = "point_id")]
		public string PointCode { get; set; }

		/// <summary>
		/// 知识点名称
		/// </summary>
		[DataMember(Name = "point_name")]
		public string PointName { get; set; }
	}
}
