using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.LoginModule.Views;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Modules.LoginModule
{
    [ModuleExport(typeof(LoginModule))]
    public class LoginModule : IModule
    {
        private static readonly Uri StatusbarViewUri = new Uri("StatusbarView", UriKind.Relative);
        [Import]
        private IRegionManager _regionManager;
        public void Initialize()
        {
            _regionManager.RequestNavigate(RegionNames.StatusbarRegion, StatusbarViewUri);
        }
    }
}
