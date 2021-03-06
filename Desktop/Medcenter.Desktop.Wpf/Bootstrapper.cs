﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.CabinetModule;
using Medcenter.Desktop.Modules.DoctorsManagerModule;
using Medcenter.Desktop.Modules.FinancesManagerModule;
using Medcenter.Desktop.Modules.PackagesManagerModule;
using Medcenter.Desktop.Modules.LoginModule;
using Medcenter.Desktop.Modules.RegistratureModule;
using Medcenter.Desktop.Modules.ScheduleManagerModule;
using Medcenter.Desktop.Modules.StatusbarModule;
using Medcenter.Desktop.Modules.SurveysManagerModule;
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
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(FinancesManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ScheduleManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(PackagesManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(SurveysManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(DoctorsManagerModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(RegistratureModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IUserRepository).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(CabinetModule).Assembly));
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
            var packagesManagerModuleType = typeof(PackagesManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = packagesManagerModuleType.Name, ModuleType = packagesManagerModuleType.AssemblyQualifiedName });
            var financesManagerModuleType = typeof(FinancesManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = financesManagerModuleType.Name, ModuleType = financesManagerModuleType.AssemblyQualifiedName });
            var doctorsManagerModuleType = typeof(DoctorsManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = doctorsManagerModuleType.Name, ModuleType = doctorsManagerModuleType.AssemblyQualifiedName });
            var userInfoModuleType = typeof(UserInfoModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = userInfoModuleType.Name, ModuleType = userInfoModuleType.AssemblyQualifiedName });
            var scheduleManagerModuleType = typeof(ScheduleManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = scheduleManagerModuleType.Name, ModuleType = scheduleManagerModuleType.AssemblyQualifiedName });
            var surveysManagerModuleType = typeof(SurveysManagerModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = surveysManagerModuleType.Name, ModuleType = surveysManagerModuleType.AssemblyQualifiedName });
            var registratureModuleType = typeof(RegistratureModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = registratureModuleType.Name, ModuleType = registratureModuleType.AssemblyQualifiedName });
            var cabinetModuleType = typeof(CabinetModule);
            ModuleCatalog.AddModule(new ModuleInfo() { ModuleName = cabinetModuleType.Name, ModuleType = cabinetModuleType.AssemblyQualifiedName });
        }
    }
}
