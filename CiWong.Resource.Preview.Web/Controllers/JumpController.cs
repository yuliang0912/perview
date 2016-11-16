using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Controllers
{
    //中转页
    public class JumpController : CustomerController
    {
        private WorkService workService;
        private WorkBaseService workBaseService;
        private PackageService packageService;

        public JumpController(WorkService _workService, WorkBaseService _workBaseService, PackageService _packageService)
        {
            this.workService = _workService;
            this.workBaseService = _workBaseService;
            this.packageService = _packageService;
        }

        //作业跳转
        [LoginAuthorize]
        public ActionResult Work(User user, long doworkId, long contentId)
        {
            var doWorkBase = workBaseService.GetDoWorkBase(doworkId);
            var workResource = workService.GetWorkResource(contentId);

            if (null == doWorkBase || null == workResource)
            {
                return RedirectToErrorAction(1, "未找到作业");
            }
            var url = string.Empty;
            if (user.UserID == doWorkBase.SubmitUserID)
            {
                url = string.Concat(RedirectHelper.GetRedirectUrl(workResource.ModuleId.ToString()), "/dowork?doworkId=", doworkId, "&contentId=", contentId);
            }
            else if (user.UserID == doWorkBase.PublishUserId)
            {
                if (workResource.ModuleId == 5 || workResource.ModuleId == 18 || workResource.ModuleId == 15)
                {
                    url = string.Concat(RedirectHelper.GetRedirectUrl(workResource.ModuleId.ToString()), "/correct?doworkId=", doworkId, "&contentId=", contentId);
                }
                else
                {
                    url = "/home/error?message=暂不支持当前作业的批改";
                }
            }
            else
            {
                url = string.Format("/home/error?message=暂无查看权限,请检查当前登录用户{0}({1})", user.UserID, user.UserName);
            }

            return Redirect(url);
        }

        /// <summary>
        /// 电子报/电子书等预览跳转
        /// 此处不做权限验证,跳转之后由具体页面验证
        /// </summary>
        public ActionResult PreView(long packageId, string cid, int? moduleId = null, long? versionId = null)
        {
            //var resourceModuleResult = packageService.GetTaskResultContents(packageId, cid).ResultContents;

            var resourceModuleResult = new List<CiWong.Tools.Package.DataContracts.TaskResultContentContract>();

            var client = new RestClient(100001);

            var packageContent = packageService.GetTaskResultContents(packageId, cid);
            //client.ExecuteGet<ReturnResult<CiWong.Tools.Package.DataContracts.PackageCategoryContentContract>>(WebApi.GetPackageCategory, new { packageId = packageId, catalogueId = cid });

            if (packageContent != null) { resourceModuleResult = packageContent.ResultContents; }

            if (null == resourceModuleResult || !resourceModuleResult.Any())
            {
                return RedirectToErrorAction(1, "暂时无法提供预览服务");
            }

            CiWong.Tools.Package.DataContracts.TaskResultContentContract resource = null;

            if (moduleId.HasValue && versionId.HasValue)
            {
                resource = resourceModuleResult.FirstOrDefault(t => t.ModuleId.Equals(moduleId) && t.ResourceVersionId.Equals(versionId));
            }
            else if (versionId.HasValue)
            {
                resource = resourceModuleResult.FirstOrDefault(t => t.ResourceVersionId.Equals(versionId));
            }
            else if (moduleId.HasValue)
            {
                resource = resourceModuleResult.FirstOrDefault(t => t.ModuleId.Equals(moduleId));
            }
            else
            {
                resource = resourceModuleResult.OrderBy(t => ResourceModuleOptions.newsPaperModuleSortArray.IndexOf(t.ModuleId)).FirstOrDefault();
            }

            if (null == resource || !RedirectHelper.IsExists(resource.ModuleId))
            {
                return RedirectToErrorAction(2, "程序暂时不支持该资源的预览");
            }
            var url = string.Format("{0}?packageId={1}&cid={2}&versionId={3}", RedirectHelper.GetRedirectUrl(resource.ModuleId.ToString()), resource.PackageId, resource.PackageCatalogueId, resource.ResourceVersionId);

            if (url.StartsWith("http://shendu.6v68.com/")) //网课
            {
                url = url + "&moduleId=" + resource.ModuleId;
            }
            return Redirect(url);
        }

        /// <summary>
        /// 单资源预览(无打包信息)
        /// </summary>
        public ActionResult Resource(string moduleId, long versionId)
        {
            if (string.IsNullOrWhiteSpace(moduleId) || versionId < 0)
            {
                return RedirectToErrorAction(1, "参数错误,请检查URL地址");
            }

            var baseUrl = RedirectHelper.GetRedirectUrl(moduleId);

            if (string.IsNullOrEmpty(baseUrl))
            {
                return RedirectToErrorAction(1, "暂时不支持该资源的预览");
            }

            if (moduleId.Equals("0b635b69-a330-4d82-8dd8-9492ca322dc5") || moduleId.Equals("4"))//课件
            {
                var resourceResult = new RestClient(100001).ExecuteGet<ReturnResult<List<ResourceAttachment>>>(WebApi.GetResourceByIds, new { ids = versionId });
                if (null == resourceResult || !resourceResult.IsSucceed || !resourceResult.Data.Any())
                {
                    return RedirectToErrorAction(1, "参数错误,请检查URL地址");
                }
                return Redirect(string.Format("http://resource.view.ciwong.com/{0}", resourceResult.Data.FirstOrDefault().source_url));
            }

            if (baseUrl.StartsWith("http://shendu.6v68.com/")) //网课
            {
                return Redirect(string.Format("{0}?versionId={1}&moduleId={2}", baseUrl, versionId, moduleId));
            }

            return Redirect(string.Format("{0}/preview?versionId={1}", baseUrl, versionId));
        }

        /// <summary>
        /// 购买资源跳转URL(优先校园书店,其次6v68)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult Buy(User user, long packageId)
        {
            var resourceResult = new RestClient(user.UserID).ExecuteGet<ReturnResult<long>>(WebApi.IsProxyProduct, new { userId = user.UserID, packageId = packageId });

            if (null != resourceResult && resourceResult.IsSucceed && resourceResult.Data > 0)
            {
                return Redirect("http://store.ciwong.com/home/bookdetail?buy=1&bookid=" + packageId);
            }
            else
            {
                return Redirect("http://www.6v68.com/Home/Search?packageId=" + packageId);
            }
        }
    }
}
