using Proiect_TW.BusinessLogic;
using Proiect_TW.Domain.Enums;
using Proiect_TW.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_TW.BusinessLogic.Interfaces;
using System.Web.Routing;



namespace Proiect_TW.Atributes
{
    public class AdminModAttribute: ActionFilterAttribute
    {
        private readonly ISession _sessionBusinessLogic;

        public AdminModAttribute()
        {
            var bl = new BussinessLogic();
            _sessionBusinessLogic = bl.GetSessionBL();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiCookie = HttpContext.Current.Request.Cookies["X-KEY"];
            if (apiCookie != null)
            {
                var profile = _sessionBusinessLogic.GetUserByCookie(apiCookie.Value);
                if (profile != null && profile.Level == URole.Admin) 
                {
                    HttpContext.Current.SetMySessionObject(profile);
                }
                else 
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "Error404" }));
                }
            }
        }
    }
}