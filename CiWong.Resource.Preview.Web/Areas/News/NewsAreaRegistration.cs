using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.News
{
    public class NewsPaperAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "News";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "News_default",
                "News/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
