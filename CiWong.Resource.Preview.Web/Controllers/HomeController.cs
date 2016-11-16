using System;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return Redirect("http://sunshine.ciwong.com");
		}

		[ResourceAuthorize(true, false)]
		public ActionResult Buy(ResourceParam baseParam, long versionId)
		{
			if (null != baseParam.PackagePermission && baseParam.PackagePermission.ExpirationDate > DateTime.Now)
			{
				return Redirect(string.Format("/jump/PreView?packageId={0}&cid={1}&versionId={2}", baseParam.PackageId, baseParam.TaskResultContent.PackageCatalogueId, baseParam.TaskResultContent.ResourceVersionId));
			}
			return View(baseParam);
		}

		[WorkAuthorize(isRedirectBuy: false)]
		public ActionResult BuyWork(WorkParam baseParam)
		{
			if (null != baseParam.PackagePermission && baseParam.PackagePermission.ExpirationDate > DateTime.Now)
			{
				return Redirect(string.Format("/jump/Work?doworkId={0}&contentId={1}", baseParam.DoWorkBase.DoWorkID, baseParam.WorkResource.ContentId));
			}
			return View(baseParam);
		}

		public ActionResult Error(int code = 0, string message = "")
		{
			ViewBag.message = message;
			return View();
		}
	}
}
