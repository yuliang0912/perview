using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Article
{
    public class ArticleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Article";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Article_default",
                "Article/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}