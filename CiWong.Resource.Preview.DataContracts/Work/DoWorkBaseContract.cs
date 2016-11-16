using System;

namespace CiWong.Resource.Preview.DataContracts
{
	/// <summary>
	/// 做作业信息
	/// </summary>
	[Serializable]
	public class DoWorkBaseContract
	{
		/// <summary>
		/// 做作业ID
		/// </summary>
		public long DoWorkID { get; set; }

		/// <summary>
		/// 作业ID（自主测试时可以为0 ）
		/// </summary>
		public long WorkID { get; set; }

		/// <summary>
		/// 作业名称
		/// </summary>
		public string WorkName { get; set; }

		/// <summary>
		/// 提交人ID
		/// </summary>
		public int SubmitUserID { get; set; }

		/// <summary>
		/// 提交人
		/// </summary>
		public string SubmitUserName { get; set; }

		/// <summary>
		/// 提交时间
		/// </summary>
		public DateTime SubmitDate { get; set; }

		/// <summary>
		/// 状态（0：未提交  1：暂存  2：已提交  3：已批改 4:退回）
		/// </summary>
		public int WorkStatus { get; set; }

		/// <summary>
		/// 评语
		/// </summary>
		public string WorkComment { get; set; }

		/// <summary>
		/// 做题方式（0：作业  1：练习）
		/// </summary>
		public int WorkPractice { get; set; }

		/// <summary>
		/// 删除状态（0：未删除  1：删除）
		/// </summary>
		public bool DelStatus { get; set; }

		/// <summary>
		/// 做作业时长（单位：分钟）
		/// </summary>
		public int WorkLong { get; set; }

		/// <summary>
		/// 实际得分
		/// </summary>
		public decimal ActualScore { get; set; }

		/// <summary>
		/// 作业总分
		/// </summary>
		public decimal WorkScore { get; set; }

		/// <summary>
		/// 作业类型
		/// </summary>
		public int WorkType { get; set; }

		/// <summary>
		/// 作业子应用
		/// </summary>
		public int SonWorkType { get; set; }

		/// <summary>
		/// 作业过期时间
		/// </summary>
		public DateTime EffectiveDate { get; set; }

		/// <summary>
		/// 作业说明
		/// </summary>
		public string WorkDesc { get; set; }

		/// <summary>
		/// 作业跳转所用参数
		/// </summary>
		public string RedirectParm { get; set; }


		/// <summary>
		/// 当前作业布置人数(兼容统计数据,数据来源于workBase)
		/// </summary>
		public int TotalNum { get; set; }

		/// <summary>
		/// 布置人ID
		/// </summary>
		public int PublishUserId { get; set; }

		/// <summary>
		/// 布置人姓名
		/// </summary>
		public string PublishUserName { get; set; }

		/// <summary>
		/// 主表状态
		/// </summary>
		public int WorkBaseStatus { get; set; }

		/// <summary>
		/// 发送日期
		/// </summary>
		public DateTime SendDate { get; set; }

		/// <summary>
		/// 是否完成作业
		/// </summary>
		public bool IsCompleted
		{
			get
			{
				return WorkStatus == 2 || WorkStatus == 3 || WorkStatus == 5;
			}
		}
	}
}
