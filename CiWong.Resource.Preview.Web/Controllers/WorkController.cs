using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Controllers
{
	[LoginAuthorize]
	public class WorkController : CustomerController
	{
		private WorkService workService;
		public WorkController(WorkService _workService)
		{
			this.workService = _workService;
		}

		/// <summary>
		/// 获取同步批次作业中未完成的单元
		/// </summary>
		public ActionResult GetNotCompletedWorks(User user, long doWorkId, long recordId)
		{
			var workResources = workService.GetWorkResources(recordId);

			var unitWorks = workService.GetUserUnitWorks(doWorkId).ToDictionary(t => t.ContentId, t => t.Status);

			var jsonData = workResources.Select(t => new
			{
				content_id = t.ContentId,
				resource_name = t.ResourceName,
				status = unitWorks.ContainsKey(t.ContentId) ? unitWorks[t.ContentId] : 0,
				url = string.Format("{0}/dowork?doworkId={1}&contentId={2}", RedirectHelper.GetRedirectUrl(t.ModuleId.ToString()), doWorkId, t.ContentId)
			});

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 通用获取答案
		/// 此方法获取到的实体对象中的answerContent为字符串,
		/// 请根据自己业务答案实体,在脚本中通过JSON.parse()转换成JSON对象
		/// </summary>
		public JsonResult GetAnswer(long doId, int answerType)
		{
			var answers = workService.GetAnswers(doId, answerType);

			return Json(new ReturnResult<List<WorkAnswerContract>>(answers));
		}

		public JsonResult Test(int moduleId = 0)
		{
			var list = workService.GetWorkCensus(new List<int> { 155014, 153812 }, Convert.ToDateTime("2014-9-1"), DateTime.Now, moduleId);

			return Json(list);
		}
	}
}
