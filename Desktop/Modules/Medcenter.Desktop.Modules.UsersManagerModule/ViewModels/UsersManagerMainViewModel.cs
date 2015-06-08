using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using ServiceStack;

namespace Medcenter.Desktop.Modules.UsersManagerModule.ViewModels
{
    [Export]
    public class UsersManagerMainViewModel: BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }  
        private readonly DelegateCommand<object> _newUserCommand;
        private readonly DelegateCommand<object> _removeUserCommand;
        private readonly DelegateCommand<object> _saveUserCommand;
        public ICommand NewUserCommand
        {
            get { return this._newUserCommand; }
        }
        public ICommand SaveUserCommand
        {
            get { return this._saveUserCommand; }
        }
        public ICommand RemoveUserCommand
        {
            get { return this._removeUserCommand; }
        }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private List<ErrorMessage> _errors;
        public List<ErrorMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        public ListCollectionView UsersFiltered
        {
            get { return _usersFiltered; }
            set { SetProperty(ref _usersFiltered, value); }
        }
        private RolesCollection _rolesDictionary;
        public RolesCollection RolesDictionary
        {
            get
            {
                return _rolesDictionary;
            }
            set
            {
                SetProperty(ref _rolesDictionary, value);
            }
        }
        private PermissionsCollection _permissionsDictionary;
        public PermissionsCollection PermissionsDictionary
        {
            get
            {
                return _permissionsDictionary;
            }
            set
            {
                SetProperty(ref _permissionsDictionary, value);
            }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        private bool _busyIndicator;
        
        private ListCollectionView _usersFiltered;

        public bool BusyIndicator
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }
        [ImportingConstructor]
        public UsersManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient)
        {
            
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _newUserCommand=new DelegateCommand<object>(NewUser);
            _removeUserCommand = new DelegateCommand<object>(RemoveUser);
            _saveUserCommand = new DelegateCommand<object>(SaveUser);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            BusyIndicator = true;
            _jsonClient.GetAsync(new UsersSelect())
            .Success(r =>
            {
                //RolesDictionary=new RolesCollection();
                BusyIndicator = false;
                Users = r.Users;
                UsersFiltered = new ListCollectionView(Users);
                SetCheckersToCurrent((User)UsersFiltered.CurrentItem);
                UsersFiltered.CurrentChanged += UsersFiltered_CurrentChanged;
            })
            .Error(ex =>
            {
                throw ex;
            });
            
        }

        private void SaveUser(object obj)
        {
            User user;
            bool isNew = UsersFiltered.CurrentAddItem != null;
            user = isNew ? (User) UsersFiltered.CurrentAddItem : (User) UsersFiltered.CurrentItem;

            user.Roles = RolesDictionary.RolesKeys;
            user.Permissions = PermissionsDictionary.PermissionsKeys;
            Errors = user.Validate();
            if (Errors.Count == 0)
            {
                BusyIndicator = true;
                _jsonClient.PostAsync(new UserSave {User = user})
                    .Success(r =>
                    {
                        BusyIndicator = false;
                        user.UserId = r.UserId;
                        user.ClearPassword();
                        if (isNew) UsersFiltered.CommitNew();
                        UsersFiltered.Refresh();
                        
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveUser(object obj)
        {
            User user;
            bool isNew = UsersFiltered.CurrentAddItem != null;
            user = isNew ? (User)UsersFiltered.CurrentAddItem : (User)UsersFiltered.CurrentItem;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Пользователь будет удалён! Вы уверены?", Title = "Удаление пользователя." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        BusyIndicator = true;
                        if (isNew)
                        {
                            UsersFiltered = new ListCollectionView(Users);
                            UsersFiltered.Refresh();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new UserDelete {Id = user.UserId})
                                .Success(r =>
                                {
                                    BusyIndicator = false;
                                    var u = Users.SingleOrDefault(i => i.UserName == user.UserName);
                                    if (u!=null) 
                                        Users.Remove(u);
                                    //UsersFiltered.CurrentChanged
                                    UsersFiltered.Refresh();
                                    UsersFiltered = new ListCollectionView(Users);
                                    //user.ClearAll();
                                    //UsersFiltered.Remove(UsersFiltered.CurrentItem);
                                    UsersFiltered.MoveCurrentToFirst();
                                    UsersFiltered.Refresh();

                                })
                                .Error(ex =>
                                {
                                    throw ex;
                                });
                        }
                    }
                });
        }

        private void NewUser(object obj)
        {
            var user = new User();
            UsersFiltered.AddNewItem(user);

        }

        void SetCheckersToCurrent(User user)
        {
            RolesDictionary = new RolesCollection(user.Roles);
            PermissionsDictionary = new PermissionsCollection(user.Permissions);
        }

        void UsersFiltered_CurrentChanged(object sender, EventArgs e)
        {
            SetCheckersToCurrent((User)UsersFiltered.CurrentItem);
            //SetProperty(ref _busyIndicator, RolesDictionary);
        }

    }
}
