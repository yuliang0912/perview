using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.News.Controllers
{
	public class HomeController : ResourceController
	{
		public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
		{
			return View(baseParam);
		}
	}
}
