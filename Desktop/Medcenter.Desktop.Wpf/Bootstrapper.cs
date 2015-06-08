using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.LoginModule;
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
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(UsersManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IUserRepository).Assembly));
        }
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new ConfigurationModuleCatalog();
        //}
        protected override void ConfigureModuleCatalog()
        {
            var loginModuleType = typeof(LoginModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = loginModuleType.Name, ModuleType = loginModuleType.AssemblyQualifiedName,});
            var usersManagerModuleType = typeof(UsersManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = usersManagerModuleType.Name, ModuleType = usersManagerModuleType.AssemblyQualifiedName, });
        }
    }
}
