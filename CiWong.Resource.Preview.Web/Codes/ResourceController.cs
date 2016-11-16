using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web
{
	/// <summary>
	/// 通用控制器基类.
	/// 请仔细了解控制器中的各个action.
	/// 业务相同的务必直接调用.
	/// 无法满足的业务允许重写
	/// </summary>
	public abstract class ResourceController : CustomerController
	{
		protected WorkService workService;
		public ResourceController()
		{
			this.workService = DependencyResolver.Current.GetService<WorkService>();
		}

		/// <summary>
		/// 自主练习/预览首页
		/// baseParam中已经包含用户信息,资源包信息,以及资源模块信息.
		/// </summary>
		[ResourceAuthorize]
		public virtual ActionResult Index(ResourceParam baseParam, long versionId)
		{
			return View(baseParam);
		}

		/// <summary>
		/// 资源预览(无打包信息)
		/// </summary>
		/// <param name="versionId"></param>
		/// <returns></returns>
		[LoginAuthorize]
		public virtual ActionResult PreView(User user, long versionId)
		{
			return View("Index", new ResourceParam() { VersionId = versionId, PackagePermission = new PackagePermissionContract() { IsFree = true }, User = user });
		}

		/// <summary>
		/// 用户做资源作业页面
		/// baseParam中已包含用户信息以及doWorkBase,workResource,unitWorks,resourceParts等必备参数
		/// WorkAuthorize过滤器已经针对用户全新,url地址等参数进行验证.
		/// </summary>
		[WorkAuthorize]
		public virtual ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
		{
			//此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
			if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
			{
				return View("WorkResult", baseParam);
			}
			return View(baseParam);
		}

		/// <summary>
		/// 提交作业
		/// 通用作业提交方法.URL中的参数固定为doWorkId,contentId 
		/// Form中为workLong(作业时长,秒).content(可选,有需要提交答案的,把答案序列化放入)
		/// </summary>
		[HttpPost, ValidateInput(false), WorkAuthorize(false, true)]
		public virtual JsonResult SubmitWork(WorkParam baseParam, long doWorkId, long contentId)
		{
			if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
			{
				//跟读作业通过接口调用,其他作业只允许提交一次
				return Json(new ReturnResult(101, "无法重复提交已完成的作业"));
			}

			if (baseParam.DoWorkBase.WorkBaseStatus != 0)
			{
				return Json(new ReturnResult(102, "这份作业已经被老师取消布置啦,再去作业系统看看有没有重新布置哦!"));
			}

			var unitWorks = baseParam.UnitWork ?? new UnitWorksContract(); //第一次提交,baseParam.UnitWork是为null.

			var actualScore = 0m;
			var content = Request.Form["content"];
			var answerList = new List<WorkAnswerContract>();

			if (!string.IsNullOrWhiteSpace(content)) //目前只做试题批改,后期有新的类型再修改
			{
				var questionCount = Convert.ToInt32(Request.Form["questionCount"]);

				var userAnswer = JSONHelper.Decode<List<WorkAnswerContract<QuestionAnswer>>>(content);

				var correctService = DependencyResolver.Current.GetService<CorrectService>();
				var wikiQues = new List<Ques.Core.Models.Question>();

				answerList = correctService.WikiQuesCorrect(userAnswer, ref actualScore, ref wikiQues);

				answerList.ForEach(t => t.ResourceType = ResourceModuleOptions.Question.ToString());

				unitWorks.CorrectRate = questionCount > 0 ? answerList.Count(t => t.Assess == 1) * 1.0m / questionCount : 0;
			}

			if (!string.IsNullOrWhiteSpace(Request.Form["workScore"]))
			{
				unitWorks.WorkScore = Convert.ToDecimal(Request.Form["workScore"]);
			}

			unitWorks.ContentId = contentId;
			unitWorks.RecordId = baseParam.WorkResource.RecordId;
			unitWorks.WorkId = baseParam.DoWorkBase.WorkID;
			unitWorks.DoWorkId = doWorkId;
			unitWorks.SubmitUserId = baseParam.User.UserID;
			unitWorks.SubmitUserName = baseParam.User.UserName;
			unitWorks.ActualScore = actualScore;
			unitWorks.SubmitDate = DateTime.Now;
			unitWorks.IsTimeOut = unitWorks.SubmitDate > baseParam.DoWorkBase.EffectiveDate;
			unitWorks.SubmitCount = unitWorks.SubmitCount + 1;
			unitWorks.Status = 3;
			unitWorks.WorkLong = Convert.ToInt32(Request["workLong"]);
			unitWorks.WorkLong = unitWorks.WorkLong < 1 ? 1 : unitWorks.WorkLong;

			var doId = workService.DoUnitWorks(unitWorks, answerList, baseParam.DoWorkBase.TotalNum);

			#region 发送消息
			if (doId > 0 && actualScore >= 70)
			{
				var msg = new CApi.Client.XiXinClient();
				var workname = "我于" + unitWorks.SubmitDate.ToString("MM月dd日 HH:mm") + "提交了“" + baseParam.WorkResource.ResourceName + "”,得分" + actualScore + ",";
				if (actualScore >= 70 && actualScore < 80)
				{
					workname += "成绩中等。";
				}
				else if (actualScore >= 80 && actualScore < 90)
				{
					workname += "成绩良好。";
				}
				else if (actualScore >= 90)
				{
					workname += "成绩优秀。";
				}
				//异步发送习信与书房消息
				System.Threading.Tasks.Task.Factory.StartNew(() => msg.SendToFamily(baseParam.User.UserID, workname));
				System.Threading.Tasks.Task.Factory.StartNew(() => SendMessage(baseParam.User, "作业消息", workname));
			}
			#endregion

			return Json(doId == 0 ? new ReturnResult("作业提交失败,请稍后再试") : new ReturnResult<long>(doId));
		}

		/// <summary>
		/// 作业结果页(目前暂不开放,使用doWork页面展示)
		/// 此页面预留给老师或者家长查看学生作业
		/// </summary>
		[WorkAuthorize]
		public virtual ActionResult WorkResult(WorkParam baseParam, long doWorkId, long contentId)
		{
			if (baseParam.workStatus != 2 && baseParam.workStatus != 3)
			{
				return View("DoWork", baseParam);
			}
			return View(baseParam);
		}

		/// <summary>
		/// 发送书房消息
		/// </summary>
		/// <returns></returns>
		protected bool SendMessage(User user, string title, string content, int messageType = 0, int isReply = 0)
		{
			var restClient = new RestClient(user.UserID);
			var result = restClient.ExecuteGet<ReturnResult<List<UserInfoDTO>>>(WebApi.GetParents, new { userId = user.UserID });

			//result.Data.Add(new UserInfoDTO() { user_id = 155014, user_name = "余亮" });
			if (null == result || !result.IsSucceed || !result.Data.Any())
			{
				return false;
			}

			foreach (var parent in result.Data)
			{
				var body = new
				{
					msg_title = title,
					msg_content = System.Web.HttpUtility.UrlEncode(content),
					receive_user_id = parent.user_id,
					receive_user_name = parent.user_name,
					message_type = messageType,
					is_reply = isReply
				};
				restClient.ExecutePost<ReturnResult<int>>(WebApi.SendMessage, body);
			}
			return true;
		}
	}
}