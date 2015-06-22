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

namespace Medcenter.Desktop.Modules.UserInfoModule.ViewModels
{
    [Export]
    public class UserInfoViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _userFotoChooseCommand;
        private readonly DelegateCommand<object> _saveUserCommand;
        private readonly DelegateCommand<object> _userEditCommand;
        private readonly DelegateCommand<object> _userReportsCommand;
        #region Properties

        public ICommand UserFotoChooseCommand
        {
            get { return this._userFotoChooseCommand; }
        }

        public ICommand SaveUserCommand
        {
            get { return this._saveUserCommand; }
        }
        public ICommand UserReportsCommand
        {
            get { return this._userReportsCommand; }
        }
        public ICommand UserEditCommand
        {
            get { return this._userEditCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
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
            }
        }
        private bool _isUserEdit;
        public bool IsUserEdit
        {
            get { return _isUserEdit; }
            set { SetProperty(ref _isUserEdit, value); }
        }
        private bool _isUserReports;
        public bool IsUserReports
        {
            get { return _isUserReports; }
            set { SetProperty(ref _isUserReports, value); }
        }
        #endregion

        [ImportingConstructor]
        public UserInfoViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _userFotoChooseCommand = new DelegateCommand<object>(UserFotoChoose);
            _saveUserCommand = new DelegateCommand<object>(SaveUser);
            _userEditCommand = new DelegateCommand<object>(UserEdit);
            _userReportsCommand = new DelegateCommand<object>(UserReports);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            _eventAggregator.GetEvent<UserInfoEvent>().Subscribe(UserInfoReceived);
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            IsUserEdit = false;
            IsUserReports = false;
            
        }

        private void UserInfoReceived(User obj)
        {
            CurrentUser = obj;
            ShowUserFoto(CurrentUser.UserId);
        }

        private void UserReports(object obj)
        {
            IsUserEdit = false;
            IsUserReports = true;
        }

        private void UserEdit(object obj)
        {
            IsUserEdit = true;
            IsUserReports = false;
        }

        private void SaveUser(object obj)
        {
            //bool isNew = CurrentUser.UserId == 0;
            Errors = CurrentUser.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new UserUpdateInfo { User = CurrentUser })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentUser.UserId = r.UserId;
                        CurrentUser.Password = "";
                        CurrentUser.Password1 = "";
                        r.Message.Message = string.Format(r.Message.Message, CurrentUser.DisplayName);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        _eventAggregator.GetEvent<UserInfoEvent>().Publish(CurrentUser);
                    })
                    .Error(ex =>
                    {
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(OperationErrors.GetErrorFromText("Сохранение изменений:", ex.Message));
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                    });
            }
        }
        private void UserFotoChoose(object obj)
        {
            string path = Utils.GetUserFotoPath(CurrentUser.UserId);
            //_isFotoChanged = true;
            ImagePath = Utils.GetUserFotoPath("NoUserFoto.png");
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
            _eventAggregator.GetEvent<UserInfoEvent>().Publish(CurrentUser);
            ShowUserFoto(CurrentUser.UserId);
        }
        private void ShowUserFoto(int userId)
        {
            string path = Utils.GetUserFotoPath(userId);
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
                        ImagePath = Utils.GetUserFotoPath("NoUserFoto.png");
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
    }
}
