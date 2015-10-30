using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Funq;
using Medcenter.Service.Interface.Services;
using Medcenter.Service.Model.Misc;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace Medcenter.Service.Local
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
    public class AppHost : AppHostHttpListenerBase //AppHostBase
    {
        public AppHost() : base("Medcenter Local Service", typeof(UserService).Assembly) { }

        public override void Configure(Container container)
        {
            //SetConfig(new HostConfig { HandlerFactoryPath = "api" });
            //ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
            var authProviders = new List<IAuthProvider> { new CredentialsAuthProvider() };
            var authFeature = new AuthFeature(SessionFactory, authProviders.ToArray());

            var connectionString = ConfigurationManager.ConnectionStrings["MedcenterDb"].ConnectionString;
            OrmLiteConfig.DialectProvider = SqlServerOrmLiteDialectProvider.Instance;
            var dbFactory = new OrmLiteConnectionFactory(connectionString);
            container.Register<IDbConnectionFactory>(dbFactory);
            var authRepo = new OrmLiteAuthRepository(dbFactory);
            //authRepo.DropAndReCreateTables();
            container.Register<IUserAuthRepository>(authRepo);
            authRepo.InitSchema();
            //container.Register(new MemoryCacheClient());
            container.RegisterAs<LogAuthEvents, IAuthEvents>();
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