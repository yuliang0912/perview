using CiWong.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Courseware
{
	/// <summary>
	/// 电子书-课件素材
	/// </summary>
	public class HomeController : ResourceController
	{
		public override ActionResult PreView(User user, long versionId)
		{
			return View("Index", new ResourceParam() { PackageType = 1, VersionId = versionId, User = user });
		}
	}
}