using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Question
{
    public class QuestionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Question";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Question_default",
                "Question/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}