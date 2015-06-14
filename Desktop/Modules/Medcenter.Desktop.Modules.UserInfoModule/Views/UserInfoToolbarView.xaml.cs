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

namespace Medcenter.Desktop.Modules.UserInfoModule.Views
{
    [Export]
    [ViewSortHint("01")]
    public partial class UserInfoToolbarView : UserControl, IPartImportsSatisfiedNotification
    {
        private static readonly Uri UserInfoViewUri = new Uri("UserInfoView", UriKind.Relative);
        private readonly IEventAggregator _eventAggregator;
        private readonly JsonServiceClient _jsonClient;
        private IRegionManager _regionManager;
        private User _currentUser;
        [ImportingConstructor]
        public UserInfoToolbarView(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);
            _eventAggregator.GetEvent<UserInfoEvent>().Subscribe(UserInfoReceived);
            InitializeComponent();
        }

        private void UserLogin(User user)
        {
            _currentUser = user;
            ChangeUserFoto();
        }

        private void UserInfoReceived(User user)
        {
            _currentUser = user;
            ChangeUserFoto();
        }

        private void ChangeUserFoto()
        {
            if (_currentUser != null)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(GetUserFotoPath(_currentUser.UserId), UriKind.Absolute);
                image.EndInit();
                UserImage.Source = image;
            }
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
            this.NavigateToUserInfoRadioButton.IsChecked = (uri == UserInfoViewUri);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, UserInfoViewUri);
            _eventAggregator.GetEvent<UserInfoEvent>().Publish(_currentUser);
        }

        private string GetUserFotoPath(int userId)
        {
            string path = (userId == 0) ? Utils.GetUserFotoPath("NoUserFoto.png") : Utils.GetUserFotoPath(userId);

            if (!File.Exists(path))
            {
                _jsonClient.GetAsync(new UserFotoDownload {UserId = userId})
                    .Success(r =>
                    {
                        if (r.FotoStream != null)
                            File.WriteAllBytes(path, r.FotoStream);
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
            return path;
        }
    }
}
