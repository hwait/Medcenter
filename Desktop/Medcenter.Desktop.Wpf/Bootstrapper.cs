using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.InspectionsManagerModule;
using Medcenter.Desktop.Modules.LoginModule;
using Medcenter.Desktop.Modules.StatusbarModule;
using Medcenter.Desktop.Modules.UserInfoModule;
using Medcenter.Desktop.Modules.UsersManagerModule;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Modularity;

namespace Medcenter.Desktop.Wpf
{
    public class Bootstrapper : MefBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (Bootstrapper).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(LoginModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(StatusbarModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(UserInfoModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(UsersManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(InspectionsManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IUserRepository).Assembly));
        }
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new ConfigurationModuleCatalog();
        //}
        protected override void ConfigureModuleCatalog()
        {
            var loginModuleType = typeof(LoginModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = loginModuleType.Name, ModuleType = loginModuleType.AssemblyQualifiedName});
            var statusbarModuleType = typeof(StatusbarModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = statusbarModuleType.Name, ModuleType = statusbarModuleType.AssemblyQualifiedName });
            var usersManagerModuleType = typeof(UsersManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = usersManagerModuleType.Name, ModuleType = usersManagerModuleType.AssemblyQualifiedName });
            var inspectionsManagerModuleType = typeof(InspectionsManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = inspectionsManagerModuleType.Name, ModuleType = inspectionsManagerModuleType.AssemblyQualifiedName });
            var userInfoModuleType = typeof(UserInfoModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = userInfoModuleType.Name, ModuleType = userInfoModuleType.AssemblyQualifiedName });
        }
    }
}
