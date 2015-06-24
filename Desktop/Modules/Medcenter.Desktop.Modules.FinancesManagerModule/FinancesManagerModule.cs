using System.ComponentModel.Composition;
using System.Linq;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.FinancesManagerModule.Views;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.FinancesManagerModule
{
    [ModuleExport(typeof(FinancesManagerModule))]
    public class FinancesManagerModule : IModule
    {
#pragma warning disable 0649,0169
        [Import]
        private IRegionManager _regionManager;

        [Import]
        private FinancesManagerToolbarView _financesManagerToolbarView;

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
                _regionManager.Regions[RegionNames.ToolbarRegion].Add(_financesManagerToolbarView, "FinancesManagerToolbarView");
            }
            else
            {
                if (_regionManager.Regions[RegionNames.ToolbarRegion].GetView("FinancesManagerToolbarView") != null)
                    _regionManager.Regions[RegionNames.ToolbarRegion].Remove(_regionManager.Regions[RegionNames.ToolbarRegion].GetView("FinancesManagerToolbarView"));
            }
        }
    }
}
