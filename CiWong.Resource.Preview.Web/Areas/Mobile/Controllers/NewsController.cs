using CiWong.Security;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Mobile.Controllers
{
    public class NewsController : ResourceController
    {
        public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
        {
            return View(baseParam);
        }

        [LoginAuthorize]
        public override ActionResult PreView(User user, long versionId)
        {
            return View(new ResourceParam() { VersionId = versionId, User = user });
        }

    }
}