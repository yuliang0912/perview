using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Listen
{
	public class ListenAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Listen";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Listen_default",
				"Listen/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}