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
using Proiect_TW.Domain.Entities.User;
using System.Data.Entity;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using NuGet.ProjectModel;
using Proiect_TW.Domain.Entities.Users;

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
            {
                cfg.CreateMap<UserLogin, ULoginData>();
                cfg.CreateMap<UserRegister, URegisterData>();
                cfg.CreateMap<UserRecoverPassword, URecoverPasswordData>();
                cfg.CreateMap<UProfileEdit, UProfileEditData>();
                cfg.CreateMap<AddProduct, ProductData>();
                cfg.CreateMap<UDbTable, UserMinimal>();
                cfg.CreateMap<Product, ProductWithPath>();
                cfg.CreateMap<Feedback, UFeedbackData>();
                cfg.CreateMap<AddOrder, OrderData>();

            });
        }
    }
}