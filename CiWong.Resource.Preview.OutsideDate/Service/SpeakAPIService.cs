
using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Service;
using CiWong.Tools.Workshop.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiWong.Resource.Preview.OutsideDate.Service
{
    /// <summary>
    /// 外部资源数据提供接口
    /// </summary>
    public class SpeakAPIService : ISpeakAPIService
    {
        #region IOC模块
        private WorkService workService;
        public SpeakAPIService(WorkService _workService)
        {
            this.workService = _workService;
        }
        #endregion

        #region 跟读作业PC端接口数据
        /// <summary>
        /// 根据课文ID取查询课文的单词 适用于练习
        /// </summary>
        /// <param name="versionId">跟读包ID</param>
        /// <returns></returns>
        public ResConents<List<WordContract>, SyncFollowReadContract> GetWordsByVersionId(long versionId)
        {
            //根据课文ID取查询课文的单词 适用于练习
            //先取课文
            var course = this.GetByVersionId<SyncFollowReadContract>(versionId).Data;

            var listWordVersionIds = course.Parts.Where(item => item.Id == "word").FirstOrDefault().List.Where(t => t.VersionId.HasValue && t.VersionId.Value > 0).Select(t => t.VersionId.Value);

            course.Parts = null;//清空数据

            //返回练习课文单词
            var resLit = this.GetByVersionIds<WordContract>(listWordVersionIds.ToArray()).Data.ToList();

            //处理音频文件为空的情况
            resLit.ForEach(t =>
            {
                if (t.AudioUrl == null || t.AudioUrl == "False")
                {
                    t.AudioUrl = "";
                }
                t.Sentences.ToList().ForEach(item =>
                {
                    if (item.AudioUrl == null || item.AudioUrl == "False")
                    {
                        item.AudioUrl = "";
					}
                });
            });

            return new ResConents<List<WordContract>, SyncFollowReadContract>()
            {
                ResEntity = course,
                ResList = resLit
            };
        }

        /// <summary>
        /// 根据课文ID取查询课文的单词 适用于作业模式
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public WorkConents<List<WordContract>> GetWordsByWorkId(long contentId, long publishId)
        {
            var workPackage = workService.GetWorkPackage(publishId);//获取作业布置信息
            var workResource = workService.GetWorkResource(contentId);//获取作业资源包信息

            if (null == workPackage || null == workResource)
            {
                throw new Exception("未找到指定的作业资源");
            }

            var words = new List<WordContract>();//定义作业单词数据列表容器

            if (workResource.IsFull) //如果子资源就取子资源内容
            {
                var wordIds = workService.GetResourceParts(contentId).Select(t => t.VersionId).ToArray();//查询子资源中单词ID列表
                words = this.GetByVersionIds<WordContract>(wordIds).Data.ToList();
            }
            else //没有布置子资源取全文单词列表
            {
                words = GetWordsByVersionId(workResource.VersionId).ResList;//查询单词列表
            }

            //处理音频文件为空的情况
            words.ForEach(t =>
            {
                if (t.AudioUrl == null || t.AudioUrl == "False")
                {
                    t.AudioUrl = "";
                }
                t.Sentences.ToList().ForEach(item =>
                {
                    if (item.AudioUrl == null || item.AudioUrl == "False")
                    {
                        item.AudioUrl = "";
                    }
                });
            });

            //返回作业单词内容
            return new CiWong.Resource.Preview.DataContracts.WorkConents<List<CiWong.Tools.Workshop.DataContracts.WordContract>>()
            {
                Content = words,
                WorkInfo = workResource,
                WorkPublishInfo = workPackage
            };
        }

        /// <summary>
        /// 根据课文ID取查询课文的句子 适用于练习
        /// </summary>
        /// <param name="versionId">课文ID</param>
        /// <returns></returns>
        public SyncFollowReadTextContract GetSentenceByVersionId(long versionId)
        {
            ////根据课文ID取查询课文的单词 适用于练习
            ////先取课文
            return this.GetByVersionId<SyncFollowReadTextContract>(versionId).Data;
        }

        /// <summary>
        /// 根据课文ID取查询课文的句子 适用于作业模式
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public WorkConents<SyncFollowReadTextContract> GetSentenceByWorkId(long contentId, long publishId)
        {
            var publishInfo = workService.GetWorkPackage(publishId);//获取作业布置信息
            var wordRescore = workService.GetWorkResource(contentId);//获取作业资源包信息

            var sentences = new SyncFollowReadTextContract();//定义句子列表容器
            if (wordRescore.IsFull) //如果有筛选取资源ID
            {
                var sentenceIds = workService.GetResourceParts(contentId).Select(t => t.VersionId).ToArray();//查询作业句子ID列表

                //查询出已布置句子列表，再取整个课文信息 取交集数据
                sentences = GetSentenceByVersionId(wordRescore.VersionId);

                //筛选已选择的句子
                sentences.Sections.SelectMany(t => t.Sentences).Where(t => t.VersionId.HasValue && sentenceIds.Contains(t.VersionId.Value)).ToList();
            }
            else
            {
                //全文布置按课文取句子
                sentences = GetSentenceByVersionId(wordRescore.VersionId);
            }

            //返回作业句子内容给客户端
            return new WorkConents<SyncFollowReadTextContract>()
            {
                Content = sentences,
                WorkInfo = wordRescore,
                WorkPublishInfo = publishInfo
            };
        }
        #endregion

        #region 听力模考PC端接口数据
        /// <summary>
        /// 根据资源ID获取听力模考试卷
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<ExaminationPaperContract> GetSimulationPaperByVersionId(long versionId)
        {
            return this.GetByVersionIds<ExaminationPaperContract>(versionId).Data.ToList();
        }

        /// <summary>
        /// 根据作业ID获取听力模考试卷
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public List<ExaminationPaperContract> GetSimulationPaperByWorkId(long contentId, long publishId, long wordId)
        {
            return null;
        }
        #endregion

        #region 资源新接口 按版本ID获取
        public dynamic GetResource(long versionId, string moduleId)
        {
            return null;
        }
        #endregion

        #region 提交作业接口数据
        /// <summary>
        /// 提交答案对象 同步跟读（单词，句子）、听力考试、模拟考试
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public bool SubmitWork(SpeekingAnswersEntity<WorkAnswerContract<ReadAnswerEntity>> answers)
        {
            if (answers == null) { return false; }

            //分享答案数据包写入答案表 此处答案方式提交都是一致的 根据类型不同提取数据
            //答案JSON编码
            answers.AnswerData.ForEach(item => { item.AnswerContent = JSONHelper.Encode<List<ReadAnswerEntity>>(item.Answers); });
            if (answers.Is_Work == 1)
            {
                var worksInfo = answers.worksInfo;
                //查询是否已做过
                var unitWork = workService.GetUserUnitWork(answers.worksInfo.ContentId, answers.worksInfo.DoWorkId);
                worksInfo.Status = 3;//手动更新状态
                worksInfo.SubmitCount = 1;
                worksInfo.WorkLong = Convert.ToInt32(worksInfo.WorkLong / 1000) + 1;
                worksInfo.WorkScore = worksInfo.ActualScore;
                if (unitWork != null)
                {
                    worksInfo.WorkScore = 100;
                    worksInfo.WorkLong = worksInfo.WorkLong + unitWork.WorkLong; //时长相加
                    worksInfo.DoId = unitWork.DoId;
                    worksInfo.Comment = unitWork.Comment;
                    worksInfo.CommentType = unitWork.CommentType;
                    if (worksInfo.ActualScore < unitWork.ActualScore)
                    {//如果成绩小于之前成绩 则沿用历史成绩
                        worksInfo.ActualScore = unitWork.ActualScore;
                    }
                    worksInfo.SubmitCount = unitWork.SubmitCount + 1;
                }
                return workService.DoUnitWorks(worksInfo, answers.AnswerData) > 0;
            }
            else
            {
                var practiceInfo = answers.practiceInfo;
                practiceInfo.Status = 3;//手动更新状态
                practiceInfo.WorkLong = Convert.ToInt32(practiceInfo.WorkLong / 1000) + 1;
                return workService.DoIndepPractice(practiceInfo, answers.AnswerData) > 0;
            }
        }

        /// <summary>
        /// 资源接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
		public dynamic GetRescoureByVersionId(int type, long versionId)
		{
			switch (type)
			{
				case 1:
					var model = this.GetByVersionId<ListeningAndSpeakingContract>(versionId);

					return model;
				case 2:
					return this.GetByVersionId<ListeningAndSpeakingContract>(versionId);
				case 3:
					return this.GetByVersionId<SyncReadContract>(versionId);
			}
			return null;
		}


        /// <summary>
        /// 新提交作业接口（业务全部在接口中实现，PC端只管理提交答案内容）
        /// </summary>
        /// <param name="answer">pc端定义的插件JSON对像</param>
        /// <param name="type"></param>
        /// <param name="versionId"></param>
        /// <param name="id"></param>
        /// <param name="isWork"></param>
        /// <returns></returns>
        public bool SubmitWorkNew(int type, int id, int isWork, int useTime, List<WorkAnswerContract> answer)
        {
            decimal score = 0;//总分

            if (isWork == 0)
            {
                var pariseWork = workService.GetIndepPracticeById(id);
                if (pariseWork != null)
                {
                    //组装答案
                    answer.ForEach(t =>
                    {
                        t.DoId = id;
                        t.AnswerType = 2;
                        t.ResourceType = "fcfd6131-cdb6-4eb8-9cb9-031f710a8f15";
                        score += t.Score;//累计分数
                    });

                    pariseWork.ActualScore = score;
                    pariseWork.Status = 1;
                    pariseWork.WorkLong = useTime;
                    pariseWork.CreateDate = DateTime.Now;

                    workService.DoIndepPractice(pariseWork, answer);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var work = workService.GetUserUnitWork(id);
                if (work != null)
                {
                    //组装答案
                    answer.ForEach(t =>
                    {
                        t.DoId = id;
                        t.AnswerType = 2;
                        t.ResourceType = "fcfd6131-cdb6-4eb8-9cb9-031f710a8f15";
                        score += t.Score;//累计分数
                    });

                    work.WorkLong = useTime;
                    work.ActualScore = score;
                    work.SubmitDate = DateTime.Now;
                    work.Status = 3;
                    work.SubmitCount = work.SubmitCount + 1;
                    workService.DoUnitWorks(work, answer);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion





        #region 私有化资源请求类
        private CiWong.Resource.Preview.DataContracts.ReturnResult<T> GetByVersionId<T>(long versionId) where T : CiWong.Tools.Workshop.DataContracts.ResourceContract, new()
        {
            var listResult = this.GetByVersionIds<T>(versionId);

            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<T>();

            if (listResult.IsSucceed)
            {
                result.Data = listResult.Data.FirstOrDefault();
                result.Code = null == result.Data ? 1 : listResult.Code;
                result.Message = null == result.Data ? "未找到资源" : result.Message;
            }
            else
            {
                result.Message = result.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>
        private CiWong.Resource.Preview.DataContracts.ReturnResult<ResourceContract> GetByVersionId(Guid moduleId, long versionId)
        {
            var toolsResult = this.GetByVersionIds(moduleId, versionId);
            //CiWong.Tools.Workshop.Services.QuestionExtensionServices.Instance.GetBy()
            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<ResourceContract>()
            {
                Data = toolsResult.Data.FirstOrDefault()
            };

            if (null == result.Data)
            {
                result.Code = 1;
                result.Message = "未找到资源";
            }
            return result;
        }



        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>
        private CiWong.Resource.Preview.DataContracts.ReturnResult<List<ResourceContract>> GetByVersionIds(Guid moduleId, params long[] versionId)
        {
            var toolsResult = CiWong.Tools.Workshop.Services.ResourceServices.Instance.GetByVersionIds(moduleId, versionId);

            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<List<ResourceContract>>()
            {
                Code = toolsResult.Code,
                Message = toolsResult.Message,
                Data = toolsResult.Data.ToList()
            };
            return result;
        }

        private CiWong.Resource.Preview.DataContracts.ReturnResult<List<T>> GetByVersionIds<T>(params long[] versionId)
            where T : CiWong.Tools.Workshop.DataContracts.ResourceContract, new()
        {
            var toolsResult = CiWong.Tools.Workshop.Services.ResourceServices.Instance.GetByVersionIds<T>(versionId);

            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<List<T>>()
            {
                Code = toolsResult.Code,
                Message = toolsResult.Message,
                Data = toolsResult.Data.ToList()
            };
            return result;
        }
        #endregion
    }
}

