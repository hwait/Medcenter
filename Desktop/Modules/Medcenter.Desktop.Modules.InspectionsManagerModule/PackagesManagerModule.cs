using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.PackagesManagerModule.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.PackagesManagerModule
{
    [ModuleExport(typeof(PackagesManagerModule))]
    public class PackagesManagerModule : IModule
    {
#pragma warning disable 0649,0169
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private PackagesManagerToolbarView _PackagesManagerToolbarView;

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
                if (user.Roles.Contains("Admin") || (user.Roles.Contains("Manager") && user.Permissions.Contains("CanEditMainLists"))) _regionManager.Regions[RegionNames.ToolbarRegion].Add(_PackagesManagerToolbarView, "PackagesManagerToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("PackagesManagerToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("PackagesManagerToolbarView"));
            }
        }
    }
}
