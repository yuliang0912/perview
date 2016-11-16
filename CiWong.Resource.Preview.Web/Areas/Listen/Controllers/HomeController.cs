using CiWong.Resource.Preview.DataContracts;
using CiWong.Security;
using System;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Listen.Controllers
{
	public class HomeController : ResourceController
	{
		public override ActionResult Index(ResourceParam baseParam, long versionId)
		{
			return View(baseParam.PackageType == 3 ? "Index" : "EbookIndex", baseParam);
		}

		/// <summary>
		/// 做作业viwe显示(重载)
		/// </summary>
		public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
		{
			//此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
			if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
			{
				return View(baseParam.PackageType == 3 ? "WorkResult" : "EbookWorkResult", baseParam);
			}
			return View(baseParam.PackageType == 3 ? "DoWork" : "EbookDoWork", baseParam);
		}

		/// <summary>
		/// 批改页面
		/// </summary>
		[WorkAuthorize]
		public ActionResult Correct(WorkParam baseParam, long doWorkId, long contentId)
		{
			if (baseParam.DoWorkBase.SubmitUserID == baseParam.User.UserID)
			{
				return Redirect(string.Format("/jump/work?doworkId={0}&contentId={1}", doWorkId, contentId));
			}
			else if (baseParam.DoWorkBase.PublishUserId != baseParam.User.UserID)
			{
				return Redirect("/home/Error?message=当前登录用户无操作权限");
			}
			return View(baseParam);
		}

		/// <summary>
		/// 练习前先创建练习记录ID，用于新版PC端作业 要求
		/// </summary>
		/// <returns>返回练习记录ID供PC端提交答案</returns>
		[LoginAuthorize]
		public JsonResult CreatePraiseWork(User user, string versionId, string moudleId, int appId, string packageId, string productId, string taskId)
		{
			return Json(workService.CreatePraiseWork(long.Parse(versionId), moudleId, appId, long.Parse(packageId), productId, user.UserID, taskId, user.UserName), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 作业前先创建作业记录ID ，用于新版PC端作业要求  1 写入作业记录信息 2 接收客户端答案  3 修改作业信息
		/// </summary>
		/// <returns></re
		[WorkAuthorize(false, true)]
		public JsonResult CreateWork(WorkParam baseParam, long contentId, long doWorkId)
		{
			if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
			{
				return Json(new ReturnResult(1, "作业已经完成啦,刷新页面试一试哦!"));
			}
			if (baseParam.UnitWork != null)
			{
				return Json(baseParam.UnitWork.DoId);
			}

			var unitWorks = new UnitWorksContract();

			unitWorks.ContentId = contentId;
			unitWorks.RecordId = baseParam.WorkResource.RecordId;
			unitWorks.WorkId = baseParam.DoWorkBase.WorkID;
			unitWorks.DoWorkId = baseParam.DoWorkBase.DoWorkID;
			unitWorks.SubmitUserId = baseParam.DoWorkBase.SubmitUserID;
			unitWorks.SubmitUserName = baseParam.DoWorkBase.SubmitUserName;
			unitWorks.SubmitDate = DateTime.Now;
			unitWorks.IsTimeOut = unitWorks.SubmitDate > baseParam.DoWorkBase.EffectiveDate;

			return Json(workService.CreateUnitWork(unitWorks));
		}
	}
}