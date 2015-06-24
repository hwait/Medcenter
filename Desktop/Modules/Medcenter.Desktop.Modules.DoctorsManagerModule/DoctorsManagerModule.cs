﻿using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.DoctorsManagerModule.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.DoctorsManagerModule
{
    [ModuleExport(typeof(DoctorsManagerModule))]
    public class DoctorsManagerModule : IModule
    {
#pragma warning disable 0649,0169
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private DoctorsManagerToolbarView _PackagesManagerToolbarView;

        [Import]
        private IEventAggregator _eventAggregator;
#pragma warning restore 0649,0169
        public void Initialize()
        {
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);

        }

        private void UserLogin(User user)
        {
            if (user != null)
            {
                if (user.Roles.Contains("Admin") || (user.Roles.Contains("Manager") && user.Permissions.Contains("CanEditMainLists"))) _regionManager.Regions[RegionNames.ToolbarRegion].Add(_PackagesManagerToolbarView, "DoctorsManagerToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("DoctorsManagerToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("DoctorsManagerToolbarView"));
            }
        }
    }
}