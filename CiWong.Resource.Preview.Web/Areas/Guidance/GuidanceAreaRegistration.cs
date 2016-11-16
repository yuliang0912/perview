using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Guidance
{
    public class GuidanceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Guidance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Guidance_default",
                "Guidance/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}