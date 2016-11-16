using CiWong.Examination.API;
using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Tools.Workshop.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiWong.Resource.Preview.OutsideDate.Service
{
    /// <summary>
    /// 外部资源数据提供接口(此服务的所有资源均可缓存)
    /// </summary>
    public class ResourceService
    {
        #region IOC模块
        private IQuesService quesService;

        public ResourceService(IQuesService _quesService)
        {
            this.quesService = _quesService;
        }
        #endregion

        #region 工具中心数据模块

        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>

        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>
        public CiWong.Resource.Preview.DataContracts.ReturnResult<ResourceContract> GetByVersionId(Guid moduleId, long versionId)
        {
            var client = new RestClient(100001);

            var result = new DataContracts.ReturnResult<ResourceContract>();

            if (moduleId == ResourceModuleOptions.TeachingPlan) //电子书 - 教学设计
            {
                var resultTeachPlan = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<TeachingPlanContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultTeachPlan.Data;
                result.Ret = resultTeachPlan.Ret;
                result.Message = resultTeachPlan.Message;
            }
            else if (moduleId == ResourceModuleOptions.Guidance) //电子书 - 导学案
            {
                var resultGuidance = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<GuidanceContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultGuidance.Data;
                result.Ret = resultGuidance.Ret;
                result.Message = resultGuidance.Message;
            }
            else if (moduleId == ResourceModuleOptions.Knowledge)//电子书 - 知识点精讲
            {
                var resultKnowledge = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<KnowledgeContent>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultKnowledge.Data;
                result.Ret = resultKnowledge.Ret;
                result.Message = resultKnowledge.Message;
            }
            else if (moduleId == ResourceModuleOptions.Courseware)//电子书 - 知识点精讲
            {
                var resultCourseware = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<CoursewareContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultCourseware.Data;
                result.Ret = resultCourseware.Ret;
                result.Message = resultCourseware.Message;
            }
            else if (moduleId == ResourceModuleOptions.News)//电子报 - 新闻
            {
                var resultNews = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<NewsContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultNews.Data;
                result.Ret = resultNews.Ret;
                result.Message = resultNews.Message;
            }
            else if (moduleId == ResourceModuleOptions.Article)//电子报 - 文章
            {
                var resultArticle = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<ArticleContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultArticle.Data;
                result.Ret = resultArticle.Ret;
                result.Message = resultArticle.Message;
            }
            else if (moduleId == ResourceModuleOptions.SyncFollowRead) //同步跟读
            {
                var resultFollowRead = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<SyncFollowReadContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultFollowRead.Data;
                result.Ret = resultFollowRead.Ret;
                result.Message = resultFollowRead.Message;
            }
            else if (moduleId == ResourceModuleOptions.SyncTrainClassHour) //同步训练 -- 课时
            {
                var resultTrain = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<SyncTrainClassHourContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultTrain.Data;
                result.Ret = resultTrain.Ret;
                result.Message = resultTrain.Message;
            }
            else if (moduleId == ResourceModuleOptions.SyncTrainSpecial)//同步训练  -- 专项
            {
                var resultSpecial = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<SyncTrainSpecialContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultSpecial.Data;
                result.Ret = resultSpecial.Ret;
                result.Message = resultSpecial.Message;
            }
            else if (moduleId == ResourceModuleOptions.SyncTrain)//电子报 - 同步训练
            {
                var resultSyncTrain = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<SyncTrainContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultSyncTrain.Data;
                result.Ret = resultSyncTrain.Ret;
                result.Message = resultSyncTrain.Message;
            }
            else if (moduleId == ResourceModuleOptions.ListeningAndSpeaking)
            {
                var resultSyncTrain = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<ListeningAndSpeakingContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                result.Data = resultSyncTrain.Data;
                result.Ret = resultSyncTrain.Ret;
                result.Message = resultSyncTrain.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据资源版本Id获取特定版本资源对象
        /// </summary>
        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>
        public string GetByVersionIds(Guid moduleId, string versionId)
        {
            var client = new RestClient(100001);

            return client.ExecuteContent(WebAPI.ResourceGetList, new { versionids = versionId, moduleid = moduleId });
        }

        /// <summary>
        /// 根据资源版本Id和模块ID获取特定版本资源对象
        /// </summary>
        public CiWong.Resource.Preview.DataContracts.ReturnResult<List<ResourceContract>> GetByVersionIds(Guid moduleId, params long[] versionId)
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

        /// <summary>
        /// 获取听说模考试卷
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract> GetListenExamination(long versionId, Guid moduleId)
        {

            var client = new RestClient(100001);
            var result = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<ListeningAndSpeakingContract>>(WebAPI.ResourceGet, new { versionid = versionId, moduleid = ResourceModuleOptions.ListeningAndSpeaking });

            if (result.Ret == 0)
            {
                return new CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract>
                {
                    Ret = result.Ret,
                    Message = result.Message,
                    Data = Infrastructure.ListenConvertHepler.ConvertSpeak(result.Data)
                };
            }
            return new CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract>
            {
                Ret = 1,
                Message = "未找到资源"
            };
        }

        #endregion

        #region 习网试卷or试题数据模块
        /// <summary>
        /// 根据试卷ID获取试卷
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns></returns> 
        public CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract> GetExamination(long examinationId)
        {
            var client = new RestClient(1000001);
            var examination = client.ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<Examination.Mapping.Entities.Examination>>(WebAPI.GetExamineeModule, new { paperId = examinationId });

            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract>
            {
                Ret = -1,
                Message = "未找到有效试卷",
                Data = null
            };
            if (examination != null)
            {
                result = new CiWong.Resource.Preview.DataContracts.ReturnResult<ExaminationContract>
                {
                    Ret = examination.Ret,
                    Message = examination.Message,
                    Data = examination.Ret == 0 ? Infrastructure.WikiQuesConvertHelper.ConvertExamination(examination.Data) : null
                };
            }
            return result;
        }

        /// <summary>
        /// 根据题目版本ID获取题目
        /// </summary>
        /// <param name="questionVersions"></param>
        /// <returns></returns>
        public CiWong.Resource.Preview.DataContracts.ReturnResult<List<CiWong.Resource.Preview.DataContracts.QuestionContract>> GetQuestions(params long[] versionId)
        {
            var versionIdList = versionId.ToList();

            var list = quesService.GetviewQuestionByVersions(versionIdList);

            if (null == list || !list.Any())
            {
                return new CiWong.Resource.Preview.DataContracts.ReturnResult<List<CiWong.Resource.Preview.DataContracts.QuestionContract>>(1, null, "未找到资源");
            }

            list = list.OrderBy(t => versionIdList.IndexOf(t.question.Version)).ToList();

            var result = new CiWong.Resource.Preview.DataContracts.ReturnResult<List<CiWong.Resource.Preview.DataContracts.QuestionContract>>()
            {
                Data = Infrastructure.WikiQuesConvertHelper.ConvertQuestionList(list)
            };

            return result;
        }
        #endregion
    }
}
