using System.Web.Mvc;
using CiWong.Resource.Preview.OutsideDate.Service;

namespace CiWong.Resource.Preview.Web
{
    /// <summary>
    /// 资源使用权限验证过滤器
    /// </summary>
    public class ResourceAuthorize : LoginAuthorize
    {
        protected ResourceParam baseParam; //基础参数
        protected bool isRedirectBuy = true;//未购买是否跳转到购买页面
        public ResourceAuthorize(bool isRedirectLogin = false, bool isRedirectBuy = true)
            : base(isRedirectLogin)
        {
            this.isRedirectBuy = isRedirectBuy;
            this.baseParam = new ResourceParam();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!isRedirectBuy && null == current)
            {
                return;
            }

            var packageService = DependencyResolver.Current.GetService<PackageService>();

            var result = filterContext.HttpContext.Request.CheckResourceParams(packageService, current);

            baseParam = result.Data;
            if (result.Code == -1)
            {
                filterContext.Result = base.RedirectLogin(filterContext);
            }
            else if (!result.IsSucceed && result.Code < 100)
            {
                filterContext.Result = new RedirectResult("/error?message=" + result.Message);
            }
            else if (!baseParam.IsCan && baseParam.PackageType == 1 && isRedirectBuy)
            {
                filterContext.Result = filterContext.HttpContext.Request.Url.ToString().LastIndexOf("mobile") > -1 ?
                 null// new RedirectResult(string.Format("/mobile/paper/buy?packageId={0}&cid={1}&versionId={2}", result.Data.PackageId, result.Data.TaskResultContent.PackageCatalogueId, result.Data.TaskResultContent.ResourceVersionId))
                   :
                  new RedirectResult(string.Format("/home/buy?packageId={0}&cid={1}&versionId={2}", result.Data.PackageId, result.Data.TaskResultContent.PackageCatalogueId, result.Data.TaskResultContent.ResourceVersionId));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("baseParam"))
            {
                filterContext.ActionParameters["baseParam"] = baseParam;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.baseParam = baseParam;
        }
    }
}