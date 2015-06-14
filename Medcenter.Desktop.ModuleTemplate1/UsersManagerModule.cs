using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.ModuleTemplate1.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.ModuleTemplate1
{
    [ModuleExport(typeof(UsersManagerModule))]
    public class UsersManagerModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private UsersManagerToolbarView _usersManagerToolbarView;

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
                if (user.Roles.Contains("Admin")) _regionManager.Regions[RegionNames.ToolbarRegion].Add(_usersManagerToolbarView, "UsersManagerToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("UsersManagerToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("UsersManagerToolbarView"));
            }
        }
    }
}
