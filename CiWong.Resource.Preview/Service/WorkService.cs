using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace CiWong.Resource.Preview.Service
{
	/// <summary>
	/// 电子报,电子书,附件作业业务处理类
	/// </summary>
	public class WorkService
	{
		private readonly WorkFilePackageRepository _workFilePackageManager = new WorkFilePackageRepository();
		private readonly PublishRecordRepository _publishRecordManager = new PublishRecordRepository();
		private readonly IndepPracticeRepository _indepPracticeManager = new IndepPracticeRepository();
		private readonly UnitWorksRepository _unitWorksManager = new UnitWorksRepository();
		private readonly FileWorksRepository _fileWorksManager = new FileWorksRepository();
		private readonly WorkAnswerRepository _workAnserManager = new WorkAnswerRepository();
		private readonly WorkBaseRepository _workBaseManager = new WorkBaseRepository();
		/// <summary>
		/// 获取打包信息
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public PublishRecordContract GetWorkPackage(long recordId)
		{
			return _publishRecordManager.GetWorkPackage(recordId);
		}

		/// <summary>
		/// 获取附件作业资源包
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public WorkFilePackageContract GetWorkFilePackage(long recordId)
		{
			return _workFilePackageManager.GetWorkFilePackage(recordId);
		}

		/// <summary>
		///  根据打包ID获取资源集合
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public List<WorkResourceContract> GetWorkResources(long recordId)
		{
			return _publishRecordManager.GetWorkResources(recordId);
		}

		/// <summary>
		///  根据打包ID获取资源集合
		/// </summary>
		/// <param name="recordIds"></param>
		/// <returns></returns>
		public List<WorkResourceContract> GetWorkResources(IEnumerable<long> recordIds)
		{
			return _publishRecordManager.GetWorkResources(recordIds);
		}

		/// <summary>
		/// 获取资源包创建记录
		/// </summary>
		/// <param name="userIds"></param>
		/// <param name="packageId"></param>
		/// <param name="cid"></param>
		/// <param name="moduleIds"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<PublishRecordContract> GetPublishRecord(IEnumerable<int> userIds, IEnumerable<int> moduleIds, long packageId, string cid, int pageSize)
		{
			return _publishRecordManager.GetPublishRecord(userIds, moduleIds, packageId, cid, pageSize);
		}


		/// <summary>
		/// 根据内容ID获取筛选子资源
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns></returns>
		public List<ResourcePartsContract> GetResourceParts(long contentId)
		{
			return _publishRecordManager.GetResourceParts(contentId);
		}

		/// <summary>
		/// 获取单元汇总完成情况
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="workId"></param>
		/// <returns></returns>
		public UnitSummaryContract GetUnitSummary(long contentId, long workId)
		{
			return _unitWorksManager.GetUnitSummary(contentId, workId);
		}

		/// <summary>
		/// 获取单元完成情况汇总
		/// </summary>
		public List<UnitSummaryContract> GetUnitSummarys(long recordId, long workId)
		{
			return _unitWorksManager.GetUnitSummarys(recordId, workId);
		}

		/// <summary>
		/// 获取完成详情集合
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="workId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUnitWorks(long contentId, long workId)
		{
			return _unitWorksManager.GetUnitWorks(contentId, workId);
		}

		/// <summary>
		/// 获取用户单个单元完成详情
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="doWorkId"></param>
		/// <returns></returns>
		public UnitWorksContract GetUserUnitWork(long contentId, long doWorkId)
		{
			return _unitWorksManager.GetUserUnitWork(contentId, doWorkId);
		}

		/// <summary>
		/// 获取一份作业的完成详情集合
		/// </summary>
		/// <param name="doWorkId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(long doWorkId)
		{
			return _unitWorksManager.GetUserUnitWorks(doWorkId);
		}

		/// <summary>
		/// 获取用户单个单元完成详情
		/// </summary>
		/// <param name="doId"></param>
		/// <returns></returns>
		public UnitWorksContract GetUserUnitWork(long doId)
		{
			return _unitWorksManager.GetUserUnitWork(doId);
		}

		/// <summary>
		/// 获取用户所有单元模块完成详情
		/// </summary>
		/// <param name="recordId"></param>
		/// <param name="workId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(long recordId, long workId, int userId)
		{
			return _unitWorksManager.GetUserUnitWorks(recordId, workId, userId);
		}

		/// <summary>
		/// 获取单元模块完成详情
		/// </summary>
		/// <param name="recordIds"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(int userId, IEnumerable<long> recordIds)
		{
			return _unitWorksManager.GetUserUnitWorks(userId, recordIds);
		}

		/// <summary>
		/// 获取附件作业完成详情集合
		/// </summary>
		public List<FileWorksContracts> GetFileWorks(long recordId, long workId)
		{
			return _fileWorksManager.GetFileWorks(recordId, workId);
		}

		/// <summary>
		/// 获取用户附件作业完成详情
		/// </summary>
		/// <returns></returns>
		public FileWorksContracts GetUserFileWork(long doWorkId)
		{
			return _fileWorksManager.GetUserFileWork(doWorkId);
		}


		/// <summary>
		/// 根据作业类型(1:作业 2:自主练习)获取答案
		/// </summary>
		public WorkAnswerContract GetAnswer(long doId, int workType, long versionId)
		{
			return _workAnserManager.GetAnswer(doId, workType, versionId);
		}

		/// <summary>
		/// 根据作业类型(1:作业 2:自主练习 3:作业快传)获取答案
		/// </summary>
		public List<WorkAnswerContract> GetAnswers(long doId, int workType)
		{
			return _workAnserManager.GetAnswers(doId, workType);
		}

		/// <summary>
		/// 批量获取作业答案
		/// </summary>
		public List<WorkAnswerContract> GetUnitWorkAnswers(long workId, long contentId, bool hasAnswerContent = false)
		{
			return _workAnserManager.GetUnitWorkAnswers(workId, contentId, hasAnswerContent);
		}

		/// <summary>
		/// 批量获取附件作业答案
		/// </summary>
		public List<WorkAnswerContract> GetFileWorkAnswers(long workId, long recordId, bool hasAnswerContent = false)
		{
			return _workAnserManager.GetFileWorkAnswers(workId, recordId, hasAnswerContent);
		}

		/// <summary>
		/// 获取打包资源集合
		/// </summary>
		public WorkResourceContract GetWorkResource(long contentId)
		{
			return _publishRecordManager.GetWorkResource(contentId);
		}

		/// <summary>
		/// 获取打包的附件资源集合
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns></returns>
		public WorkFileResourceContract GetWorkFileResource(long contentId)
		{
			return _workFilePackageManager.GetWorkFileResource(contentId);
		}

		/// <summary>
		/// 创建作业打包记录
		/// </summary>
		public long CreateWorkPackage(PublishRecordContract model)
		{
			long recordId = 0;
			using (var trans = new TransactionScope())
			{
				//打包
				recordId = _publishRecordManager.InsertPublishRecord(model);
				//打包资源
				foreach (var item in model.workResources)
				{
					item.RecordId = recordId;
					item.IsFull = item.resourceParts.Any();
					_publishRecordManager.InsertWorkResource(item);
					item.resourceParts.ToList().ForEach(t => t.ContentId = item.ContentId);
				}
				_publishRecordManager.InsertResourceParts(model.workResources.SelectMany(t => t.resourceParts));
				trans.Complete();
			}
			return recordId;
		}

		/// <summary>
		/// 创建作业附件包
		/// </summary>
		public long CreateWorkFilePackage(WorkFilePackageContract model)
		{
			if (null == model || !model.WorkFileResources.Any())
			{
				throw new ArgumentNullException("附件资源包为null或者附件资源包中不包含任何附件");
			}
			long recordId = 0;
			using (var trans = new TransactionScope())
			{
				model.CreateDate = DateTime.Now;
				//打包
				recordId = _workFilePackageManager.InsertWorkFilePackage(model);
				_workFilePackageManager.InsertWorkFileResources(model.WorkFileResources, recordId);
				trans.Complete();
			}
			return recordId;
		}

		/// <summary>
		/// 获取作业附件集
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public List<WorkFileResourceContract> GetWorkFileResources(long recordId)
		{
			return _workFilePackageManager.GetWorkFileResources(recordId);
		}

		/// <summary>
		/// 获取作业附件集
		/// </summary>
		/// <param name="recordIds"></param>
		/// <returns></returns>
		public List<WorkFileResourceContract> GetWorkFileResources(IEnumerable<long> recordIds)
		{
			if (null == recordIds || !recordIds.Any())
			{
				return new List<WorkFileResourceContract>();
			}
			return _workFilePackageManager.GetWorkFileResources(recordIds);
		}

		/// <summary>
		///  自主练习
		/// </summary>
		public long DoIndepPractice(IndepPracticeContract model, IEnumerable<WorkAnswerContract> userAnswer = null)
		{
			long doId = 0;
			using (var trans = new TransactionScope())
			{
				if (model != null)
				{
					doId = model.Id;
					model.CreateDate = DateTime.Now;
					if (doId == 0)
					{
						doId = _indepPracticeManager.Insert(model);
					}
					else
					{
						_indepPracticeManager.UpDate(model);
					}
					if (doId > 0)
					{
						_workAnserManager.Insert(doId, 2, userAnswer);
					}
				}
				trans.Complete();
			}
			return doId;
		}

		/// <summary>
		/// 提交单元作业
		/// </summary>
		public long DoUnitWorks(UnitWorksContract model, IEnumerable<WorkAnswerContract> userAnswer = null, int totalNum = 0)
		{
			if (model == null)
			{
				throw new ArgumentNullException("参数model为null");
			}
			using (var trans = new TransactionScope())
			{
				model.SubmitDate = DateTime.Now;
				if (model.DoId == 0)
				{
					_unitWorksManager.Insert(model);
				}
				else
				{
					_unitWorksManager.Update(model);
				}
				_workAnserManager.Insert(model.DoId, 1, userAnswer);
				trans.Complete();
			}

			if (model.DoId > 0)
			{
				if (totalNum < 1)
				{
					var workInfo = _workBaseManager.GetWorkBase(model.WorkId);
					if (workInfo != null)
					{
						totalNum = workInfo.TotalNum;
					}
				}
				//异步更新统计数据
				System.Threading.Tasks.Task.Factory.StartNew(() =>_unitWorksManager.SetUnitSummary(model.ContentId, model.WorkId, model.RecordId, totalNum));
				System.Threading.Tasks.Task.Factory.StartNew(() => this.UpdateDoWorkInfo(model.DoWorkId, model.RecordId, model.SubmitDate));
			}

			return model.DoId;
		}

		/// <summary>
		/// 提交附件作业
		/// </summary>
		/// <param name="model"></param>
		/// <param name="userAnswer"></param>
		/// <returns></returns>
		public long DoFileWork(FileWorksContracts model, IEnumerable<WorkAnswerContract> userAnswer = null)
		{
			if (model == null)
			{
				throw new ArgumentNullException("参数model为null");
			}
			using (var trans = new TransactionScope())
			{
				model.SubmitDate = DateTime.Now;
				if (model.DoId == 0)
				{
					_fileWorksManager.Insert(model);
				}
				else
				{
					_fileWorksManager.Update(model);
				}
				_workAnserManager.Insert(model.DoId, 3, userAnswer);
				trans.Complete();
			}
			if (model.DoId > 0)
			{
				_workBaseManager.UpdateDoWorkStatus(model.DoWorkId, model.SubmitDate, model.Status, model.WorkLong, 0m);
			}
			return model.DoId;
		}

		/// <summary>
		/// 作业点评
		/// </summary>
		/// <param name="userIds"></param>
		/// <param name="workId"></param>
		/// <param name="contentId"></param>
		/// <param name="content"></param>
		/// <param name="commentType"></param>
		/// <returns></returns>
		public bool CommentUnitWorks(IEnumerable<int> userIds, long workId, long contentId, string content, int commentType)
		{
			return _unitWorksManager.CommentUnitWorks(userIds, workId, contentId, content, commentType);
		}

		/// <summary>
		/// 批量点评附件作业
		/// </summary>
		/// <param name="userIds">用户更ID</param>
		/// <param name="workId">作业ID</param>
		/// <param name="recordId">附件资源包ID</param>
		/// <param name="workLevel">作业评分</param>
		/// <param name="content">点评内容</param>
		/// <param name="commentType">点评类型</param>
		/// <returns></returns>
		public bool CommentFileWorks(IEnumerable<int> userIds, long workId, long recordId, decimal workLevel, string content, int commentType)
		{
			if (null == userIds || !userIds.Any())
			{
				return false;
			}

			_workBaseManager.UpdateDoWorkStatus(userIds, workId, 3, workLevel);

			return _fileWorksManager.CommentFileWorks(userIds, workId, recordId, workLevel, content, commentType);
		}

		/// <summary>
		/// 获取单个实体
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public IndepPracticeContract GetIndepPracticeById(long Id)
		{
			return _indepPracticeManager.GetPracticeById(Id);
		}

		/// <summary>
		/// 更新作业系统做作业信息
		/// </summary>
		/// <param name="doWorkId"></param>
		/// <param name="recordId"></param>
		/// <param name="submitDate"></param>
		/// <returns></returns>
		private bool UpdateDoWorkInfo(long doWorkId, long recordId, DateTime submitDate)
		{
			if (_unitWorksManager.IsCompletedAllUnits(recordId, doWorkId))
			{
				return _workBaseManager.UpdateDoWorkStatus(doWorkId, submitDate, 3, 0, 0);
			}
			return false;
		}

		/// <summary>
		/// 获取跟读答案
		/// </summary>
		public List<WorkAnswerContract<FlowReadAnswerEntity>> GetReadAnswers(long doId, int answerType)
		{
			return _workAnserManager.GetReadAnswers(doId, answerType);
		}

		/// <summary>
		/// 获取使用情况统计
		/// </summary>
		public List<WorkCensusContract> GetWorkCensus(IEnumerable<int> userIdList, DateTime beginDate, DateTime endDate, int moduleId = 0)
		{
			//作业数据
			var workCensus = _unitWorksManager.GetWorkCensus(userIdList, beginDate, endDate, moduleId).ToDictionary(c => c.UserId, c => c);

			//自主练习数据
			var indeppracticeCensus = new Dictionary<int, WorkCensusContract>();

			if (moduleId == 0 || moduleId == 10)
			{
				_indepPracticeManager.GetIndeppracticeCensus(userIdList, beginDate, endDate, "992a5055-e9d0-453f-ab40-666b4d7030bb", "f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35").ToDictionary(c => c.UserId, c => c);
			}

			foreach (var item in workCensus)
			{
				if (indeppracticeCensus.ContainsKey(item.Key))
				{
					item.Value.TotalSubmitNum += indeppracticeCensus[item.Key].TotalWorkNum;
					item.Value.TotalWorkLong += indeppracticeCensus[item.Key].TotalWorkLong;
					item.Value.TotalWorkNum += indeppracticeCensus[item.Key].TotalWorkNum;
				}
			}

			foreach (var item in indeppracticeCensus)
			{
				if (!workCensus.ContainsKey(item.Key))
				{
					workCensus.Add(item.Key, item.Value);
				}
			}

			return workCensus.Select(t => t.Value).ToList();
		}

		/// <summary>
		/// 批改作业答案
		/// </summary>
		/// <param name="workAnswer"></param>
		/// <returns></returns>
		public bool CorrectAnswer(UnitWorksContract unitWork, WorkAnswerContract workAnswer)
		{
			using (var trans = new TransactionScope())
			{
				_workAnserManager.CorrectAnswer(workAnswer);
				_unitWorksManager.Update(unitWork);
				trans.Complete();
			}
			return true;
		}


		/// <summary>
		/// 批改附件作业
		/// </summary>
		/// <param name="workId"></param>
		/// <param name="doWorkId"></param>
		/// <param name="userId"></param>
		/// <param name="workAnswer"></param>
		/// <returns></returns>
		public bool CorrectFileWorkAnswer(long workId, long doWorkId, int userId, WorkAnswerContract workAnswer)
		{
			_workBaseManager.UpdateDoWorkStatus(new List<int>() { userId }, workId, 3, -1m);

			_fileWorksManager.UpdateStatus(doWorkId, 3);

			return _workAnserManager.CorrectAnswer(workAnswer);
		}


		#region 听力模考作业
		public long CreateUnitWork(UnitWorksContract unitwork)
		{
			return _unitWorksManager.Insert(unitwork);
		}

		public long CreatePraiseWork(long verionId, string moduleId, int appId, long packageId, string productId, int userId, string taskId, string userName)
		{
			//先验证作业是否已创建
			var unitWork = _indepPracticeManager.GetPracticeByIds(verionId, moduleId, userId);
			if (unitWork != null && unitWork.Id > 0)
			{
				return unitWork.Id;
			}

			return _indepPracticeManager.Insert(new IndepPracticeContract()
			{
				CreateDate = Convert.ToDateTime("1990-01-01"),
				ActualScore = 0,
				AppId = appId,
				CorrectRate = 0,
				ModuleId = moduleId,
				PackageId = packageId,
				PackageName = "",
				PackageType = 0,
				ProductId = productId,
				ResourceName = "",
				ResourceType = 0,
				Status = 0,
				SubmitUserId = userId,
				SubmitUserName = userName,
				TaskId = taskId,
				VersionId = verionId,
				WorkLong = 0,
				WorkScore = 0
			});
		}
		#endregion

	}
}

