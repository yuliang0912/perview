using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Epaper.Controllers
{
    public class SyncRepeatController : ResourceController
    {
        public ActionResult WordPreview(long versionId)
        {
            return View(new ResourceParam() { VersionId = versionId });
        }

        public ActionResult TextPreview(long versionId)
        {
            return View(new ResourceParam() { VersionId = versionId });
        }

        public ActionResult ArticlePreview(long versionId)
        {
            return View(new ResourceParam() { VersionId = versionId });
        }
    }
}