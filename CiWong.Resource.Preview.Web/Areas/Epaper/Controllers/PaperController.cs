using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Epaper.Controllers
{
    public class PaperController : ResourceController
    {

        public ActionResult paperPreview(long versionId, int moduleId)
        {
            if (moduleId == 15)
            {
                ViewBag.ModuleId = "e9430760-9f2e-4256-af76-3bd8980a9de4";
            }
            else
            {
                ViewBag.ModuleId = "1f693f76-02f5-4a40-861d-a8503df5183f";
            }
            return View("Index", new ResourceParam() { VersionId = versionId, PackagePermission = new PackagePermissionContract() { IsFree = true } });
        }
    }
}