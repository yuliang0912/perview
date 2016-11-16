using System;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Mobile.Controllers
{
    /// <summary>
    /// 移动端试卷模块
    /// </summary>
    public class PaperController : ResourceController
    {
        //
        // GET: /Mobile/Home/
        public override ActionResult Index(ResourceParam baseParam, long versionId)
        {
            return View(baseParam);
        }

        [ResourceAuthorize(true, false)]
        public ActionResult Buy(ResourceParam baseParam, long versionId)
        {
            if (baseParam.IsCan)
            {
                return Redirect(string.Format("/jump/PreView?packageId={0}&cid={1}&versionId={2}", baseParam.PackageId, baseParam.TaskResultContent.PackageCatalogueId, baseParam.TaskResultContent.ResourceVersionId));
            }
            return View(baseParam);
        }

        [WorkAuthorize(isRedirectBuy: false)]
        public ActionResult BuyWork(WorkParam baseParam)
        {
            if (baseParam.IsCan)
            {
                return Redirect(string.Format("/jump/Work?doworkId={0}&contentId={1}", baseParam.DoWorkBase.DoWorkID, baseParam.WorkResource.ContentId));
            }
            return View(baseParam);
        }


        /// <summary>
        /// 做作业viwe显示(重载)
        /// </summary>
        public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
        {
            //此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
            if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
            {
                return View("WorkResult", baseParam);
            }
            return View("DoWork", baseParam);
        }

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
    }
}