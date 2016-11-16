using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CiWong.Resource.Preview.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("resourcePreview");
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            MvcHandler.DisableMvcResponseHeader = true;//隐藏ASP.NET MVC的版本信息，使其不在HTTP Header中显示。
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutofacConfig.RegisterIOC();

            //ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            //ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var err = HttpContext.Current.Server.GetLastError();
            if (err == null) return;
            Logger.Error(string.Format("  错误地址:{0}\r\n 异常消息:{1}\r\n{2}\r\n", HttpContext.Current.Request.Url, err.Message, err.StackTrace));
        }
    }
}