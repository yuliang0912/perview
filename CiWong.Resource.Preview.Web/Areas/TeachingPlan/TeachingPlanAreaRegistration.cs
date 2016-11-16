using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.TeachingPlan
{
    public class TeachingPlanAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TeachingPlan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TeachingPlan_default",
                "TeachingPlan/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
