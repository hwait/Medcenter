using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Win32;
using ServiceStack;

namespace Medcenter.Desktop.ModuleTemplate1.ViewModels
{
    [Export]
    public class UsersManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _userFotoChooseCommand;
        private readonly DelegateCommand<object> _newUserCommand;
        private readonly DelegateCommand<object> _removeUserCommand;
        private readonly DelegateCommand<object> _saveUserCommand;
        private bool _isFotoChanged = false;
        private readonly string _defaultImagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Fotos\\NoUserFoto.png";
        #region Properties

        public ICommand NewUserCommand
        {
            get { return this._newUserCommand; }
        }
        public ICommand UserFotoChooseCommand
        {
            get { return this._userFotoChooseCommand; }
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

        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
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
            get { return _rolesDictionary; }
            set { SetProperty(ref _rolesDictionary, value); }
        }

        private PermissionsCollection _permissionsDictionary;

        public PermissionsCollection PermissionsDictionary
        {
            get { return _permissionsDictionary; }
            set { SetProperty(ref _permissionsDictionary, value); }
        }
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                SetProperty(ref _imagePath, value);
            }
        }
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                SetProperty(ref _currentUser, value);
                _newUserCommand.RaiseCanExecuteChanged();
                SetCheckersToCurrent(_currentUser);
            }
        }

        private bool _busyIndicator;

        private ListCollectionView _usersFiltered;

        public bool BusyIndicator
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }

        #endregion

        [ImportingConstructor]
        public UsersManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            ImagePath = GetUserFotoPath("NoUserFoto.png");
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _userFotoChooseCommand = new DelegateCommand<object>(UserFotoChoose);
            _newUserCommand = new DelegateCommand<object>(NewUser, CanAddUser);
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

                UsersFiltered.CurrentChanged += UsersFiltered_CurrentChanged;
            })
            .Error(ex =>
            {
                throw ex;
            });

        }



        private bool CanAddUser(object arg)
        {
            return CurrentUser == null || CurrentUser.UserId != 0;
        }

        private void SaveUser(object obj)
        {
            User user;
            bool isNew = CurrentUser.UserId == 0;
            CurrentUser.Roles = RolesDictionary.RolesKeys;
            CurrentUser.Permissions = PermissionsDictionary.PermissionsKeys;
            Errors = CurrentUser.Validate();
            if (Errors.Count == 0)
            {
                BusyIndicator = true;
                _jsonClient.PostAsync(new UserSave { User = CurrentUser })
                    .Success(r =>
                    {
                        BusyIndicator = false;
                        CurrentUser.UserId = r.UserId;
                        CurrentUser.Password = "";
                        CurrentUser.Password1 = "";
                        //CurrentUser.ClearPassword();
                        if (isNew)
                        {
                            UsersFiltered.AddNewItem(CurrentUser);
                            UsersFiltered.CommitNew();
                        }
                        UsersFiltered.Refresh();
                        r.Message.Message = string.Format(r.Message.Message, CurrentUser.DisplayName);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _newUserCommand.RaiseCanExecuteChanged();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveUser(object obj)
        {
            bool isNew = CurrentUser.UserId == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Пользователь будет удалён! Вы уверены?", Title = "Удаление пользователя." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        BusyIndicator = true;
                        if (isNew)
                        {
                            CurrentUser = new User();
                            _newUserCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new UserDelete { Id = CurrentUser.UserId })
                            .Success(r =>
                            {
                                BusyIndicator = false;
                                r.Message.Message = string.Format(r.Message.Message, CurrentUser.DisplayName);
                                UsersFiltered.Remove(UsersFiltered.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                _newUserCommand.RaiseCanExecuteChanged();
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
            CurrentUser = new User();
            //UsersFiltered.AddNewItem(user);

        }

        void SetCheckersToCurrent(User user)
        {
            RolesDictionary = new RolesCollection(user.Roles);
            PermissionsDictionary = new PermissionsCollection(user.Permissions);
        }

        void UsersFiltered_CurrentChanged(object sender, EventArgs e)
        {
            CurrentUser = UsersFiltered.CurrentItem != null ? (User)UsersFiltered.CurrentItem : new User();
            ShowUserFoto(CurrentUser.UserId);

            //SetCheckersToCurrent(CurrentUser);
            //SetProperty(ref _busyIndicator, RolesDictionary);
        }
        private void UserFotoChoose(object obj)
        {
            string path = GetUserFotoPath(CurrentUser.UserId);
            _isFotoChanged = true;
            ImagePath = GetUserFotoPath("NoUserFoto.png");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (var fileStream = File.OpenRead(openFileDialog.FileName))
                {
                    var r = _jsonClient.PostFileWithRequest<UserFotoUploadResponse>(fileStream, "none", new UserFotoUpload { UserId = CurrentUser.UserId });
                    _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                    using (var file = File.Create(path))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        fileStream.CopyTo(file);
                    }
                }
            }
            ShowUserFoto(CurrentUser.UserId);
        }
        private void ShowUserFoto(int userId)
        {
            string path = GetUserFotoPath(userId);
            if (CurrentUser.UserId == 0) return;
            if (!File.Exists(path))
            {
                _jsonClient.GetAsync(new UserFotoDownload { UserId = CurrentUser.UserId })
                .Success(r =>
                {
                    if (r.FotoStream != null)
                    {
                        File.WriteAllBytes(path, r.FotoStream);
                        ImagePath = path;
                    }
                    else
                    {
                        ImagePath = _defaultImagePath;
                    }
                })
                .Error(ex =>
                {
                    throw ex;
                });
            }
            else
            {
                ImagePath = path;
            }
        }

        private string GetUserFotoPath(int userId)
        {
            return string.Format("{0}Fotos\\{1}.jpg", AppDomain.CurrentDomain.BaseDirectory, userId);
        }
        private string GetUserFotoPath(string file)
        {
            return string.Format("{0}Fotos\\{1}", AppDomain.CurrentDomain.BaseDirectory, file);
        }
    }
}
