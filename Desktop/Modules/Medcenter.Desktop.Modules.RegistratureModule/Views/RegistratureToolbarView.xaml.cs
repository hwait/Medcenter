using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Medcenter.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Regions;

namespace Medcenter.Desktop.Modules.RegistratureModule.Views
{
    [Export]
    [ViewSortHint("07")]
    public partial class RegistratureToolbarView : UserControl, IPartImportsSatisfiedNotification
    {
        private static readonly Uri RegistratureMainViewUri = new Uri("RegistratureMainView", UriKind.Relative);
        private IRegionManager _regionManager;
        [ImportingConstructor]
        public RegistratureToolbarView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();
        }
        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion != null && mainRegion.NavigationService != null)
            {
                mainRegion.NavigationService.Navigated += this.MainRegion_Navigated;
            }
        }

        public void MainRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }

        private void UpdateNavigationButtonState(Uri uri)
        {
            this.NavigateToRegistratureRadioButton.IsChecked = (uri == RegistratureMainViewUri);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {


            _regionManager.RequestNavigate(RegionNames.MainRegion, RegistratureMainViewUri);
        }
    }
}
