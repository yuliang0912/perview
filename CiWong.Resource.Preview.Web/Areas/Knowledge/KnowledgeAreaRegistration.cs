using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Knowledge
{
    public class KnowledgeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Knowledge";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Knowledge_default",
                "Knowledge/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}