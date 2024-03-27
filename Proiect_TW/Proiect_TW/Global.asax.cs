using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Optimization;
using AutoMapper;
using Proiect_TW.Web.Models.Users;
using Proiect_TW.BusinessLogic.Entities.User;

namespace Proiect_TW
{

    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeAutoMapper();

        }
        protected static void InitializeAutoMapper()
        {
            Mapper.Initialize(cfg =>

                cfg.CreateMap<UserLogin, ULoginData>()
            );
        }
    }
}