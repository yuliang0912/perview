using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using CiWong.Resource.Preview.Common;

namespace CiWong.Resource.Preview.Web.Areas.Question.Controllers
{
	public class HomeController : ResourceController
	{
		private CorrectService correctService;
		private WorkBaseService workBaseService;

		public HomeController(CorrectService _correctService, WorkBaseService _workBaseService)
		{
			this.correctService = _correctService;
			this.workBaseService = _workBaseService;
		}

		[HttpPost, ValidateInput(false)]
		public JsonResult Correct()
		{
			var answerContent = Request.Form["content"];

			List<WorkAnswerContract<QuestionAnswer>> userAnswer = null;
			try
			{
				userAnswer = JSONHelper.Decode<List<WorkAnswerContract<QuestionAnswer>>>(answerContent);
			}
			catch (Exception e)
			{
				return Json(new ReturnResult("数据格式错误,exception:" + e.ToString()));
			}

			var totalScore = 0m;
			var wikiQues = new List<Ques.Core.Models.Question>();

			var result = correctService.WikiQuesCorrect(userAnswer, ref totalScore, ref wikiQues);

			return Json(new ReturnResult<List<WorkAnswerContract>>(result));
		}

		[HttpPost, ValidateInput(false)]
		public JsonResult Correct1(List<WorkAnswerContract<QuestionAnswer>> content)
		{
			return Json(1);
		}

		/// <summary>
		/// 试题批改
		/// </summary>
		/// <param name="doId">做作业ID</param>
		/// <param name="versionId">试题版本ID</param>
		/// <param name="sid">选项下标</param>
		/// <param name="assess">结果</param>
		/// <param name="itemScore">选项得分</param>
		/// <returns></returns>
		[LoginAuthorize]
		public JsonResult SaveCorrect(User user, long doId, long versionId, int sid, int assess, decimal itemScore, Guid moduleId)
		{
			var unitWork = workService.GetUserUnitWork(doId);
			if (null == unitWork)
			{
				return Json(new ReturnResult(1, "未找到指定的作业!"));
			}
			var doWorkBase = workBaseService.GetDoWorkBase(unitWork.DoWorkId);
			if (doWorkBase.PublishUserId != user.UserID)
			{
				return Json(new ReturnResult(2, string.Format("暂无当前作业批改权限.登陆ID:{0}({1})!", user.UserID, user.UserName)));
			}
			if (unitWork.Status != 2 && unitWork.Status != 3)
			{
				return Json(new ReturnResult(3, "当前作业尚未完成,您还不能批改!"));
			}

			var workAnswer = workService.GetAnswer(doId, 1, versionId);

			var oldScore = null == workAnswer ? 0m : workAnswer.Score;

			if (moduleId == ResourceModuleOptions.ListeningAndSpeakingExam)
			{
				workAnswer = listenPaperCorrect(workAnswer, doId, versionId, sid, assess, itemScore);
			}
			else
			{
				workAnswer = paperCorrect(workAnswer, doId, versionId, sid, assess, itemScore);
			}
		
			unitWork.ActualScore += workAnswer.Score - oldScore;

			if (workService.CorrectAnswer(unitWork, workAnswer))
			{
				return Json(new ReturnResult<decimal>(unitWork.ActualScore));
			}
			else
			{
				return Json(new ReturnResult(4, "保存批改失败"));
			}
		}

		private WorkAnswerContract paperCorrect(WorkAnswerContract workAnswer, long doId, long versionId, int sid, int assess, decimal itemScore)
		{
			if (null == workAnswer)
			{
				var questionAnswers = new List<QuestionAnswer>()
				{
					new QuestionAnswer()
					{
						Sid = sid,
						Assess = assess,
						ItemScore = itemScore,
						Content = string.Empty
					}
				};
				workAnswer = new WorkAnswerContract()
				{
					DoId = doId,
					VersionId = versionId,
					AnswerType = 1,
					ResourceType = ResourceModuleOptions.Question.ToString(),
					Assess = assess,
					Score = itemScore,
					AnswerContent = JSONHelper.Encode<List<QuestionAnswer>>(questionAnswers)
				};
			}
			else
			{
				var questionAnswers = JSONHelper.Decode<List<QuestionAnswer>>(workAnswer.AnswerContent);
				var currAnswer = questionAnswers.FirstOrDefault(t => t.Sid == sid);
				if (null == currAnswer)
				{
					questionAnswers.Add(new QuestionAnswer()
					{
						Sid = sid,
						Assess = assess,
						ItemScore = itemScore,
						Content = string.Empty
					});
				}
				else
				{
					currAnswer.Assess = assess;
					currAnswer.ItemScore = itemScore;
				}

				if (questionAnswers.Count == 1)
				{
					workAnswer.Assess = questionAnswers[0].Assess;
				}
				else if (!questionAnswers.Any(t => t.Assess != 1))
				{
					workAnswer.Assess = 1;
				}
				else if (questionAnswers.Any(t => t.Assess == 2))
				{
					workAnswer.Assess = 2;
				}
				else if (!questionAnswers.Any(t => t.Assess != 3))
				{
					workAnswer.Assess = 3;
				}
				else if (questionAnswers.Any(t => t.Assess == 4))
				{
					workAnswer.Assess = 4;
				}
				workAnswer.Score = questionAnswers.Sum(t => t.ItemScore);
				workAnswer.AnswerContent = JSONHelper.Encode<List<QuestionAnswer>>(questionAnswers);
			}
			return workAnswer;
		}


		private WorkAnswerContract listenPaperCorrect(WorkAnswerContract workAnswer, long doId, long versionId, int sid, int assess, decimal itemScore)
		{
			if (null == workAnswer)
			{
				var questionAnswers = new List<ListenAnswerEntity>()
				{
					new ListenAnswerEntity()
					{
						Sid = sid,
						AudioUrl = string.Empty,
						BlankContent = string.Empty
					}
				};
				workAnswer = new WorkAnswerContract()
				{
					DoId = doId,
					VersionId = versionId,
					AnswerType = 1,
					ResourceType = ResourceModuleOptions.Question.ToString(),
					Assess = assess,
					Score = itemScore,
					AnswerContent = JSONHelper.Encode<List<ListenAnswerEntity>>(questionAnswers)
				};
			}
			else
			{
                var questionAnswers = JSONHelper.Decode<List<ListenAnswerEntity>>(workAnswer.AnswerContent);
                if (null == questionAnswers || !questionAnswers.Any())
                {
                    questionAnswers.Add(new ListenAnswerEntity()
                    {
                        Sid = sid,
                        AudioUrl = string.Empty,
                        BlankContent = string.Empty
                    });
                    workAnswer.AnswerContent = JSONHelper.Encode<List<ListenAnswerEntity>>(questionAnswers);
                }
                workAnswer.Assess = assess;
                workAnswer.Score = itemScore;
			}
			return workAnswer;
		}
	}
}
