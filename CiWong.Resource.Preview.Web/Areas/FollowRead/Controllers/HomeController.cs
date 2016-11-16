using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.FollowRead.Controllers
{
	public class HomeController : ResourceController
	{
		public override ActionResult Index(ResourceParam baseParam, long versionId)
		{
			return View(baseParam.PackageType == 3 ? "Index" : "EbookIndex", baseParam);
		}

		public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
		{
			//此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
			if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
			{
				return View(baseParam.PackageType == 3 ? "WorkResult" : "EbookWorkResult", baseParam);
			}
			return View(baseParam.PackageType == 3 ? "DoWork" : "EbookDoWork", baseParam);
		}
	}
}