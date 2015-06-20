﻿using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.InspectionsManagerModule.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.InspectionsManagerModule
{
    [ModuleExport(typeof(InspectionsManagerModule))]
    public class InspectionsManagerModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private InspectionsManagerToolbarView _InspectionsManagerToolbarView;

        [Import]
        private IEventAggregator _eventAggregator;
        public void Initialize()
        {
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);

        }

        private void UserLogin(User user)
        {
            if (user != null)
            {
                if (user.Roles.Contains("Admin") || (user.Roles.Contains("Manager") && user.Permissions.Contains("CanEditMainLists"))) _regionManager.Regions[RegionNames.ToolbarRegion].Add(_InspectionsManagerToolbarView, "InspectionsManagerToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("InspectionsManagerToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("InspectionsManagerToolbarView"));
            }
        }
    }
}