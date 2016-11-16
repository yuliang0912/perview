using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.SyncTrain
{
    public class NewsPaperAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SyncTrain";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SyncTrain_default",
                "SyncTrain/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
