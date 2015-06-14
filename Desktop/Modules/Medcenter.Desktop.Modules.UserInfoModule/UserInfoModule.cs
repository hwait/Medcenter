using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.UserInfoModule.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.UserInfoModule
{
    [ModuleExport(typeof(UserInfoModule))]
    public class UserInfoModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private UserInfoToolbarView _UserInfoToolbarView;

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
                _regionManager.Regions[RegionNames.ToolbarRegion].Add(_UserInfoToolbarView, "UserInfoToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("UserInfoToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("UserInfoToolbarView"));
            }
        }
    }
}
