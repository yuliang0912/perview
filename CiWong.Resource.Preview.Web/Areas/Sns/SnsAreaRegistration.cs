using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Sns
{
    public class SnsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sns";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sns_default",
                "Sns/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
