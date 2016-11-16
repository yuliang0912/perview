using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Mobile.Controllers
{
    public class ListenController : ResourceController
    {
        /// <summary>
        /// 批改页面
        /// </summary>
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

        /// <summary>
        /// 做作业viwe显示(重载)
        /// </summary>
        public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
        {
            //此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
            if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
            {
                return View("WorkResult", baseParam);//作业快传听说模块的电子报和电子书共用一个视图页面。baseParam.PackageType == 3 ? "WorkResult" : "EbookWorkResult"
            }
            return View(baseParam.PackageType == 3 ? "DoWork" : "EbookDoWork", baseParam);//这段代码不会执行，暂时不管
        }
	}
}