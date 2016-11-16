using CiWong.Resource.Preview.DataContracts;
using CiWong.Tools.Workshop.DataContracts;
using System.Collections.Generic;

namespace CiWong.Resource.Preview.OutsideDate.Service
{
	/// <summary>
	/// 外部资源数据提供接口
	/// </summary>
	public interface ISpeakAPIService
	{
		#region 跟读作业PC端接口数据
		/// <summary>
		/// 根据课文ID取查询课文的单词 适用于练习
		/// </summary>
		/// <param name="versionId"></param>
		/// <returns></returns>
		ResConents<List<WordContract>, SyncFollowReadContract> GetWordsByVersionId(long versionId);


		/// <summary>
		/// 根据课文ID取查询课文的单词 适用于作业模式
		/// </summary>
		/// <param name="workId"></param>
		/// <returns></returns>
		WorkConents<List<WordContract>> GetWordsByWorkId(long contentId, long publishId);


		/// <summary>
		/// 根据课文ID取查询课文的句子 适用于练习
		/// </summary>
		/// <param name="versionId"></param>
		/// <returns></returns>
		SyncFollowReadTextContract GetSentenceByVersionId(long versionId);


		/// <summary>
		/// 根据课文ID取查询课文的句子 适用于作业模式
		/// </summary>
		/// <param name="workId"></param>
		/// <returns></returns>
		WorkConents<SyncFollowReadTextContract> GetSentenceByWorkId(long contentId, long publishId);
		#endregion

		#region 听力模考作业
		/// <summary>
		/// 根据资源ID获取听力模考试卷
		/// </summary>
		/// <param name="versionId"></param>
		/// <returns></returns>
		List<CiWong.Tools.Workshop.DataContracts.ExaminationPaperContract> GetSimulationPaperByVersionId(long versionId);
		/// <summary>
		/// 根据作业ID获取听力模考试卷
		/// </summary>
		/// <param name="workId"></param>
		/// <returns></returns>
		List<CiWong.Tools.Workshop.DataContracts.ExaminationPaperContract> GetSimulationPaperByWorkId(long contentId, long publishId, long wordId);
		#endregion

		#region 提交作业及练习
		/// <summary>
		/// 提交答案对象
		/// </summary>
		/// <param name="answer"></param>
		/// <returns></returns>
		bool SubmitWork(SpeekingAnswersEntity<WorkAnswerContract<ReadAnswerEntity>> answer);

		/// <summary>
		/// 查询作业是否已做过
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		//UnitWorksContract GetUnitWorkByIdByContendId(long contentId, int userId);

		//新版资源拉取20140826
		dynamic GetRescoureByVersionId(int type, long versionId);
		//新版作业提交20140826
		bool SubmitWorkNew(int type, int id, int isWork, int useTime, List<WorkAnswerContract> answer);
		#endregion
	}
}
