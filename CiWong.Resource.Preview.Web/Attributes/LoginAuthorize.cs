using CiWong.Resource.Preview.DataContracts;
using CiWong.Security;
using CiWong.Security.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web
{
    /// <summary>
    /// 用户登录验证过滤器
    /// </summary>
    public class LoginAuthorize : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        protected bool isRedirect;
        protected User current;
		protected IAuthenticationService authenticationService { get; set; }

		public LoginAuthorize(bool isRedirect = true)
		{
			this.isRedirect = isRedirect;
			this.authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
		}

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (authenticationService.Current.IsAuthenticated) //验证通过
            {
                current = authenticationService.Current;
            }
            else if (isRedirect) //验证不通过并跳转到登陆页面
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new CustomJsonResult() { Data = new ReturnResult(-1, "未检测到登录用户,请登录后再试!") };
                }
                else
                {
					filterContext.Result = RedirectLogin(filterContext);
                }
            }
            else //验证不通过也不跳转到登陆页面
            {
                current = new User();
            }
        }

		public RedirectResult RedirectLogin(AuthorizationContext filterContext)
		{
			var reurl = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri.ToLower();
			return new RedirectResult("https://passport.ciwong.com/signin?returnUrl=" + HttpUtility.UrlEncode(reurl, System.Text.Encoding.UTF8));
		}

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.User = current;
                filterContext.Controller.ViewBag.AuthID = current.UserID;
            }
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("user"))
            {
                filterContext.ActionParameters["user"] = current;
            }
        }
    }
}