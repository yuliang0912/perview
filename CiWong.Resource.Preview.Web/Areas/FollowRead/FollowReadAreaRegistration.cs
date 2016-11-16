using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.FollowRead
{
	public class FollowReadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
				return "FollowRead";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
				"FollowRead_default",
				"FollowRead/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}