using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Epaper.Controllers
{
    public class SyncPracticeController : ResourceController
    {
        public ActionResult practiceView(long versionId)
        {
            return View("Index", new ResourceParam() { VersionId = versionId, PackagePermission = new PackagePermissionContract() { IsFree = true } });
        }

    }
}