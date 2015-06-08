using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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

        private readonly IRegionManager _regionManager;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        [Export(typeof(JsonServiceClient))]
        public JsonServiceClient JsonClient;
        [ImportingConstructor]
        public LoginFormViewModel(IRegionManager regionManager, IUserRepository userRepository, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UserLoginEvent>().Subscribe(UserLogin);

            LoginCommand = new DelegateCommand<object>(TryLogin, CanLogin);
            JsonClient = new JsonServiceClient("http://Nikk-PC/Medcenter.Service.MVC5/api/");
            BusyIndicator = true;
            JsonClient.GetAsync(new UserSelect())
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
                _regionManager.RequestNavigate(RegionNames.MainRegion, welcomeViewUri);
            }
            else
            {
                _regionManager.Regions[RegionNames.ToolbarRegion].Remove(ServiceLocator.Current.GetInstance<LogoutToolbarView>());
                _regionManager.RequestNavigate(RegionNames.MainRegion, loginFormViewUri);
                Password = "";
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

       
        public DelegateCommand<object> LoginCommand { get; set; }
        

        private bool CanLogin(object arg)
        {
            return true;
        }

        private void TryLogin(object obj)
        {

            var authResponse = JsonClient.Send(new Authenticate
            {
                provider = CredentialsAuthProvider.Name,
                UserName = LoginSelected,
                Password = _password,
                RememberMe = true,
            });
            CurrentUser = new User();
            CurrentUser.UserName = LoginSelected;
            CurrentUser.UserId = int.Parse(authResponse.UserId);
            CurrentUser.DisplayName = authResponse.DisplayName;
            CurrentUser.SessionId = authResponse.SessionId;
            //CurrentUser.Roles=authResponse.
            JsonClient.GetAsync(new RolesSelect())
            .Success(r =>
            {
                CurrentUser.Roles = r.Roles;
                _eventAggregator.GetEvent<UserLoginEvent>().Publish(CurrentUser);
            })
            .Error(ex =>
            {
                throw ex;
            });
            // Get User from repo, with Roles 
            //CurrentUser.Roles = "Manager";
            //if (authResponse.ResponseStatus.ErrorCode=="") _eventAggregator.GetEvent<UserLoginEvent>().Publish(CurrentUser);
        }
    }
}
