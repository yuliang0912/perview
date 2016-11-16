using CiWong.Security;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Knowledge.Controllers
{
    public class HomeController : ResourceController
    {
		public override ActionResult PreView(User user, long versionId)
		{
			return View("Index", new ResourceParam() { PackageType = 1, VersionId = versionId, User = user });
		}
    }
}
