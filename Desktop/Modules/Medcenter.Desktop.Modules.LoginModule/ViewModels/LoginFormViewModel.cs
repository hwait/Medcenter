using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Desktop.Modules.LoginModule.Views;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using ServiceStack;
using ServiceStack.Auth;

namespace Medcenter.Desktop.Modules.LoginModule.ViewModels
{
    [Export]
    public class LoginFormViewModel : BindableBase
    {
        private static Uri welcomeViewUri = new Uri("WelcomeMainView", UriKind.Relative);
        private static Uri loginFormViewUri = new Uri("LoginFormView", UriKind.Relative);
        private static readonly Uri StatusbarViewUri = new Uri("StatusbarView", UriKind.Relative);
        private readonly IRegionManager _regionManager;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        [Export(typeof(JsonServiceClient))]
        public JsonServiceClient JsonClient;
        [ImportingConstructor]
        public LoginFormViewModel(IRegionManager regionManager, IUserRepository userRepository, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);
            
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, Utils.TimerShowMessage);
            

            LoginCommand = new DelegateCommand<object>(TryLogin, CanLogin);
            JsonClient = new JsonServiceClient("http://Nikk-PC/Medcenter.Service.MVC5/api/");

            RefreshData();
        }

        

        private void RefreshData()
        {
            BusyIndicator = true;
            JsonClient.GetAsync(new LoginsSelect())
            .Success(r =>
            {
                BusyIndicator = false;
                Users = r.Users;
            })
            .Error(ex => { throw ex; });
        }
        private void UserLogin(User user)
        {
            if (user != null)
            {
                _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof (LogoutToolbarView));
                _regionManager.RequestNavigate(RegionNames.StatusbarRegion, StatusbarViewUri);
                _regionManager.RequestNavigate(RegionNames.MainRegion, welcomeViewUri);
            }
            else
            {
                _regionManager.Regions[RegionNames.ToolbarRegion].Remove(ServiceLocator.Current.GetInstance<LogoutToolbarView>());
                _regionManager.RequestNavigate(RegionNames.MainRegion, loginFormViewUri);
                Password = "";
                CurrentUser = null;
                JsonClient.Post(new Authenticate { provider = AuthenticateService.LogoutAction });
                RefreshData();
            }
        }
        private bool _busyIndicator;
        public bool BusyIndicator
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }
        private ObservableCollection<string> _users;
        public ObservableCollection<string> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        public string LoginSelected { get; set; }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }
       
        public DelegateCommand<object> LoginCommand { get; set; }
        

        private bool CanLogin(object arg)
        {
            return true;
        }

        private void TryLogin(object obj)
        {
            try
            {
                var authResponse = JsonClient.Send(new Authenticate
                {
                    provider = CredentialsAuthProvider.Name,
                    UserName = LoginSelected,
                    Password = _password,
                    //RememberMe = true,
                });
                JsonClient.GetAsync(new UserSelect { UserId = int.Parse(authResponse.UserId) })
                .Success(ru =>
                {
                    CurrentUser = ru.User;
                    CurrentUser.SessionId = authResponse.SessionId;
                    JsonClient.GetAsync(new RolesSelect {DeviceId = "Dev1"})
                        .Success(rr =>
                        {
                            CurrentUser.Roles = new ObservableCollection<string>(rr.Roles);
                            JsonClient.GetAsync(new PermissionsSelect {})
                                .Success(rp =>
                                {
                                    CurrentUser.Permissions = new ObservableCollection<string>(rp.Permissions);
                                    _eventAggregator.GetEvent<UserLoginEvent>().Publish(CurrentUser);
                                })
                                .Error(ex =>
                                {
                                    throw ex;
                                });
                        })
                        .Error(ex =>
                        {
                            throw ex;
                        });

                })
                .Error(ex =>
                {
                    throw ex;
                });
            }
            catch (WebServiceException webEx)
            {
                ErrorMessage = "Ошибка авторизации. Неправильный пароль или пользователь заблокирован.";
                _dispatcherTimer.Start();
            }
            

            // Get User from repo, with Roles 
            //CurrentUser.Roles = "Manager";
            //if (authResponse.ResponseStatus.ErrorCode=="") _eventAggregator.GetEvent<UserLoginEvent>().Publish(CurrentUser);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            ErrorMessage = "";
            Password = "";
            (sender as DispatcherTimer).Stop();
        }
    }
}
