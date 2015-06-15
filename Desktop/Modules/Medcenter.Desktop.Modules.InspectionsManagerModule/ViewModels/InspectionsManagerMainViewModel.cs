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

namespace Medcenter.Desktop.Modules.InspectionsManagerModule.ViewModels
{
    [Export]
    public class InspectionsManagerMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _addInspectionToGroupCommand;
        private readonly DelegateCommand<object> _removeInspectionFromGroupCommand;
        private readonly DelegateCommand<object> _newInspectionCommand;
        private readonly DelegateCommand<object> _removeInspectionCommand;
        private readonly DelegateCommand<object> _saveInspectionCommand;
        private readonly DelegateCommand<object> _newInspectionGroupCommand;
        private readonly DelegateCommand<object> _removeInspectionGroupCommand;
        private readonly DelegateCommand<object> _saveInspectionGroupCommand;
        #region Properties

        public ICommand AddInspectionToGroupCommand
        {
            get { return this._addInspectionToGroupCommand; }
        }
        public ICommand RemoveInspectionFromGroupCommand
        {
            get { return this._removeInspectionFromGroupCommand; }
        }
        public ICommand NewInspectionCommand
        {
            get { return this._newInspectionCommand; }
        }
        public ICommand RemoveInspectionCommand
        {
            get { return this._removeInspectionCommand; }
        }
        public ICommand SaveInspectionCommand
        {
            get { return this._saveInspectionCommand; }
        }
        public ICommand NewInspectionGroupCommand
        {
            get { return this._newInspectionGroupCommand; }
        }
        public ICommand RemoveInspectionGroupCommand
        {
            get { return this._removeInspectionGroupCommand; }
        }
        public ICommand SaveInspectionGroupCommand
        {
            get { return this._saveInspectionGroupCommand; }
        }

        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private ListCollectionView _inspectionGroups;
        public ListCollectionView InspectionGroups
        {
            get { return _inspectionGroups; }
            set { SetProperty(ref _inspectionGroups, value); }
        }
        private ListCollectionView _inspections;
        public ListCollectionView Inspections
        {
            get { return _inspections; }
            set { SetProperty(ref _inspections, value); }
        }

        private Inspection _currentInspection;

        public Inspection CurrentInspection
        {
            get { return _currentInspection; }
            set
            {
                SetProperty(ref _currentInspection, value);
            }
        }
        private InspectionGroup _currentInspectionGroup;

        public InspectionGroup CurrentInspectionGroup
        {
            get { return _currentInspectionGroup; }
            set
            {
                SetProperty(ref _currentInspectionGroup, value);
            }
        }
        private bool _busyIndicator;

        public bool BusyIndicator
        {
            get { return _busyIndicator; }
            set { SetProperty(ref _busyIndicator, value); }
        }
        #endregion

        [ImportingConstructor]
        public InspectionsManagerMainViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _removeInspectionFromGroupCommand = new DelegateCommand<object>(RemoveInspectionFromGroup);
            _addInspectionToGroupCommand = new DelegateCommand<object>(AddInspectionToGroup);
            _newInspectionCommand = new DelegateCommand<object>(NewInspection);
            _removeInspectionCommand = new DelegateCommand<object>(RemoveInspection);
            _saveInspectionCommand = new DelegateCommand<object>(SaveInspection);
            _newInspectionGroupCommand = new DelegateCommand<object>(NewInspectionGroup);
            _removeInspectionGroupCommand = new DelegateCommand<object>(RemoveInspectionGroup);
            _saveInspectionGroupCommand = new DelegateCommand<object>(SaveInspectionGroup);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();

            BusyIndicator = true;
            //_jsonClient.GetAsync(new UsersSelect())
            //.Success(r =>
            //{
            //    //RolesDictionary=new RolesCollection();
            //    BusyIndicator = false;
            //    Users = r.Users;
            //    UsersFiltered = new ListCollectionView(Users);

            //    UsersFiltered.CurrentChanged += UsersFiltered_CurrentChanged;
            //})
            //.Error(ex =>
            //{
            //    throw ex;
            //});

        }

        

        #region InspectionGroup

        private void NewInspectionGroup(object obj)
        {
            throw new NotImplementedException();
        }

        private void SaveInspectionGroup(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveInspectionGroup(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Inspection

        private void SaveInspection(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveInspection(object obj)
        {
            throw new NotImplementedException();
        }

        private void NewInspection(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Inspections in Group

        private void AddInspectionToGroup(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveInspectionFromGroup(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion


        //private void SaveUser(object obj)
        //{
        //    User user;
        //    bool isNew = CurrentUser.UserId == 0;
        //    CurrentUser.Roles = RolesDictionary.RolesKeys;
        //    CurrentUser.Permissions = PermissionsDictionary.PermissionsKeys;
        //    Errors = CurrentUser.Validate();
        //    if (Errors.Count == 0)
        //    {
        //        BusyIndicator = true;
        //        _jsonClient.PostAsync(new UserSave { User = CurrentUser })
        //            .Success(r =>
        //            {
        //                BusyIndicator = false;
        //                CurrentUser.UserId = r.UserId;
        //                CurrentUser.Password = "";
        //                CurrentUser.Password1 = "";
        //                //CurrentUser.ClearPassword();
        //                if (isNew)
        //                {
        //                    UsersFiltered.AddNewItem(CurrentUser);
        //                    UsersFiltered.CommitNew();
        //                }
        //                UsersFiltered.Refresh();
        //                r.Message.Message = string.Format(r.Message.Message, CurrentUser.DisplayName);
        //                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
        //                _newUserCommand.RaiseCanExecuteChanged();
        //            })
        //            .Error(ex =>
        //            {
        //                throw ex;
        //            });
        //    }
        //}

        //private void RemoveUser(object obj)
        //{
        //    bool isNew = CurrentUser.UserId == 0;
        //    ConfirmationRequest.Raise(
        //        new Confirmation { Content = "Пользователь будет удалён! Вы уверены?", Title = "Удаление пользователя." },
        //        c =>
        //        {
        //            if (c.Confirmed)
        //            {
        //                BusyIndicator = true;
        //                if (isNew)
        //                {
        //                    CurrentUser = new User();
        //                    _newUserCommand.RaiseCanExecuteChanged();
        //                }
        //                else
        //                {
        //                    _jsonClient.GetAsync(new UserDelete { Id = CurrentUser.UserId })
        //                    .Success(r =>
        //                    {
        //                        BusyIndicator = false;
        //                        r.Message.Message = string.Format(r.Message.Message, CurrentUser.DisplayName);
        //                        UsersFiltered.Remove(UsersFiltered.CurrentItem);
        //                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
        //                        _newUserCommand.RaiseCanExecuteChanged();
        //                    })
        //                    .Error(ex =>
        //                    {
        //                        throw ex;
        //                    });
        //                }
        //            }
        //        });
        //}
    }
}
