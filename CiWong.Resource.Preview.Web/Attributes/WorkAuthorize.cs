using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Resource.Preview.Service;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web
{
    /// <summary>
    /// 作业权限验证过滤器
    /// </summary>
    public class WorkAuthorize : LoginAuthorize
    {
        protected WorkParam baseParam; //基础参数
        private bool isAjax = false;
        protected bool isRedirectBuy = true;//未购买是否跳转到购买页面

        public WorkAuthorize(bool isRedirect = true, bool isAjax = false, bool isRedirectBuy = true)
            : base(isRedirect)
        {
            this.isAjax = isAjax;
            this.isRedirectBuy = isRedirectBuy;
            this.baseParam = new WorkParam();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (null == current || current.UserID == 0)
            {
                if (isAjax || !isRedirectBuy)
                {
                    filterContext.Result = new CustomJsonResult() { Data = new ReturnResult(-1, "未检测到登录用户,请登录后再试!") };
                }
                else
                {
                    baseParam.User = current;
                }
                return;
            }

            var workBaseService = (WorkBaseService)DependencyResolver.Current.GetService<WorkBaseService>();
            var workService = (WorkService)DependencyResolver.Current.GetService<WorkService>();
            var packageService = DependencyResolver.Current.GetService<PackageService>();

            var result = filterContext.HttpContext.Request.CheckWorkParams(current, workBaseService, workService, packageService, !isAjax, isRedirectBuy);

            baseParam = result.Data;

            if (result.Code != 0 && result.Code < 100 && !isAjax)//参数或权限验证失败
            {
                filterContext.Result = new RedirectResult("/error?message=" + result.Message);
            }
            else if (result.Code != 0 && result.Code < 100)
            {
                filterContext.Result = new CustomJsonResult() { Data = result };
            }
            else if (result.Code != 0 && isAjax)
            {
				filterContext.Result = new CustomJsonResult() { Data = new ReturnResult(-1, result.Message) };
            }
            else if (!baseParam.IsCan && baseParam.PackageType == 1 && isRedirectBuy && 5 != baseParam.WorkResource.ModuleId)
            {
                filterContext.Result = filterContext.Result = filterContext.HttpContext.Request.Url.ToString().LastIndexOf("mobile") > -1 ?
                 null //new RedirectResult(string.Format("/mobile/paper/buyWork?doworkId={0}&contentId={1}", result.Data.DoWorkBase.DoWorkID, result.Data.WorkResource.ContentId))
                 : new RedirectResult(string.Format("/home/buyWork?doworkId={0}&contentId={1}", result.Data.DoWorkBase.DoWorkID, result.Data.WorkResource.ContentId));
            }
            baseParam.ReturnResult = result;
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
            if (!isAjax)
            {
                filterContext.Controller.ViewBag.baseParam = baseParam;
            }
        }
    }
}