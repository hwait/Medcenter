using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using ServiceStack;

namespace Medcenter.Desktop.Modules.FinancesManagerModule.Views
{
    [Export]
    [ViewSortHint("03")]
    public partial class FinancesManagerToolbarView : UserControl, IPartImportsSatisfiedNotification
    {
        private static readonly Uri FinancesManagerViewUri = new Uri("FinancesManagerView", UriKind.Relative);
        private readonly IEventAggregator _eventAggregator;
        private readonly JsonServiceClient _jsonClient;
        private IRegionManager _regionManager;
        private User _currentUser;
        [ImportingConstructor]
        public FinancesManagerToolbarView(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);
            //_eventAggregator.GetEvent<FinancesManagerEvent>().Subscribe(FinancesManagerReceived);
            InitializeComponent();
        }

        private void UserLogin(User user)
        {
            _currentUser = user;
        }

        //private void FinancesManagerReceived(Discount discount)
        //{
        //    _currentDiscount = discount;
        //    ChangeDiscountFoto();
        //}

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
            this.NavigateToFinancesManagerRadioButton.IsChecked = (uri == FinancesManagerViewUri);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, FinancesManagerViewUri);
            //_eventAggregator.GetEvent<FinancesManagerEvent>().Publish(_currentDiscount);
        }
    }
}
