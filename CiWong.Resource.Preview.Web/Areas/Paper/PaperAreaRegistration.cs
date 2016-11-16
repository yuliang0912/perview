using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Paper
{
    public class PaperAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Paper";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Paper_default",
                "Paper/{action}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
