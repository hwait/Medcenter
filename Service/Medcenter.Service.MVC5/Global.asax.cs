using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Funq;
using Medcenter.Service.Interface;
using Medcenter.Service.Interface.Services;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using ServiceStack.OrmLite.SqlServer;

namespace Medcenter.Service.MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            new AppHost().Init();
        }
    }
    public class AppHost : AppHostHttpListenerBase //AppHostBase
    {
        public AppHost() : base("Medcenter Web Service", typeof(UserService).Assembly) { }

        public override void Configure(Container container)
        {
            SetConfig(new HostConfig { HandlerFactoryPath = "api" });
            ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
            var authProviders = new List<IAuthProvider> { new CredentialsAuthProvider() };
            var authFeature = new AuthFeature(SessionFactory, authProviders.ToArray());
            
            var connectionString = ConfigurationManager.ConnectionStrings["MedcenterDb"].ConnectionString;
            OrmLiteConfig.DialectProvider =  SqlServerOrmLiteDialectProvider.Instance;
            var dbFactory = new OrmLiteConnectionFactory(connectionString);
            container.Register<IDbConnectionFactory>(dbFactory);
            var authRepo = new OrmLiteAuthRepository(dbFactory);
            //authRepo.DropAndReCreateTables();
            container.Register<IUserAuthRepository>(authRepo);
            authRepo.InitSchema();
            //container.Register(new MemoryCacheClient());
            Plugins.Add(authFeature);
            Plugins.Add(new CustomRegistrationValidator.CustomRegistrationFeature());
            Plugins.Add(new PostmanFeature());
            //SetConfig(new HostConfig { DebugMode = true });
        }
        private IAuthSession SessionFactory()
        {
            return new AuthUserSession();
        }

    }
}
