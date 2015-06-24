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

namespace Medcenter.Desktop.Modules.FinancesManagerModule.ViewModels
{
    [Export]
    public class FinancesManagerViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly JsonServiceClient _jsonClient;
        private readonly IEventAggregator _eventAggregator;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }
        private readonly DelegateCommand<object> _newDiscountCommand;
        private readonly DelegateCommand<object> _copyDiscountCommand;
        private readonly DelegateCommand<object> _removeDiscountCommand;
        private readonly DelegateCommand<object> _saveDiscountCommand;
        private readonly DelegateCommand<object> _discountEditCommand;
        private readonly DelegateCommand<object> _comissionFeesCommand;
        #region Properties
        public ICommand DiscountEditCommand
        {
            get { return this._discountEditCommand; }
        }

        public ICommand ComissionFeesCommand
        {
            get { return this._comissionFeesCommand; }
        }
        public ICommand NewDiscountCommand
        {
            get { return this._newDiscountCommand; }
        }

        public ICommand SaveDiscountCommand
        {
            get { return this._saveDiscountCommand; }
        }
        public ICommand CopyDiscountCommand
        {
            get { return this._copyDiscountCommand; }
        }
        public ICommand RemoveDiscountCommand
        {
            get { return this._removeDiscountCommand; }
        }
        private List<ResultMessage> _errors;

        public List<ResultMessage> Errors
        {
            get { return _errors; }
            set { SetProperty(ref _errors, value); }
        }
        private Discount _currentDiscount;

        public Discount CurrentDiscount
        {
            get { return _currentDiscount; }
            set
            {
                SetProperty(ref _currentDiscount, value);
            }
        }
        private ListCollectionView _discounts;
        public ListCollectionView Discounts
        {
            get { return _discounts; }
            set { SetProperty(ref _discounts, value); }
        }
        private bool _isDiscountEdit;
        public bool IsDiscountEdit
        {
            get { return _isDiscountEdit; }
            set { SetProperty(ref _isDiscountEdit, value); }
        }
        private bool _isComissionFees;
        public bool IsComissionFees
        {
            get { return _isComissionFees; }
            set { SetProperty(ref _isComissionFees, value); }
        }
        #endregion

        [ImportingConstructor]
        public FinancesManagerViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {
            /*
              private readonly DelegateCommand<object> _newDiscountCommand;
        private readonly DelegateCommand<object> _copyDiscountCommand;
        private readonly DelegateCommand<object> _removeDiscountCommand;
        private readonly DelegateCommand<object> _saveDiscountCommand;
             */
            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _newDiscountCommand = new DelegateCommand<object>(NewDiscount);
            _saveDiscountCommand = new DelegateCommand<object>(SaveDiscount);
            _copyDiscountCommand = new DelegateCommand<object>(CopyDiscount);
            _removeDiscountCommand = new DelegateCommand<object>(RemoveDiscount);
            _comissionFeesCommand = new DelegateCommand<object>(ComissionFees);
            _discountEditCommand = new DelegateCommand<object>(DiscountEdit);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            //_eventAggregator.GetEvent<FinancesManagerEvent>().Subscribe(FinancesManagerReceived);
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            _jsonClient.GetAsync(new DiscountsSelect())
            .Success(r =>
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                Discounts = new ListCollectionView(r.Discounts);
                Discounts.CurrentChanged += Discounts_CurrentChanged;
            })
            .Error(ex =>
            {
                throw ex;
            });
            
            IsDiscountEdit = false;
            IsComissionFees = false;

        }
        void Discounts_CurrentChanged(object sender, EventArgs e)
        {
            CurrentDiscount = Discounts.CurrentItem != null ? (Discount)Discounts.CurrentItem : new Discount();
        }
        //private void FinancesManagerReceived(Discount obj)
        //{
        //    CurrentDiscount = obj;
        //    ShowDiscountFoto(CurrentDiscount.DiscountId);
        //}

        private void ComissionFees(object obj)
        {
            IsDiscountEdit = false;
            IsComissionFees = true;
        }

        private void DiscountEdit(object obj)
        {
            IsDiscountEdit = true;
            IsComissionFees = false;
        }
        private void CopyDiscount(object obj)
        {
            CurrentDiscount = CurrentDiscount.CopyInstance();
        }
        private void SaveDiscount(object obj)
        {
            bool isNew = CurrentDiscount.Id == 0;
            Errors = CurrentDiscount.Validate();
            if (Errors.Count == 0)
            {
                _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                _jsonClient.PostAsync(new DiscountSave { Discount = CurrentDiscount })
                    .Success(r =>
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                        CurrentDiscount.Id = r.DiscountId;
                        if (isNew)
                        {
                            Discounts.AddNewItem(CurrentDiscount);
                            Discounts.CommitNew();
                        }
                        Discounts.Refresh();
                        r.Message.Message = string.Format(r.Message.Message, CurrentDiscount.Name);
                        _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                        //_newUserCommand.RaiseCanExecuteChanged();
                    })
                    .Error(ex =>
                    {
                        throw ex;
                    });
            }
        }

        private void RemoveDiscount(object obj)
        {
            bool isNew = CurrentDiscount.Id == 0;
            ConfirmationRequest.Raise(
                new Confirmation { Content = "Скидка будет удалена! Вы уверены?", Title = "Удаление скидки." },
                c =>
                {
                    if (c.Confirmed)
                    {
                        _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
                        if (isNew)
                        {
                            CurrentDiscount = new Discount();
                            //_newUserCommand.RaiseCanExecuteChanged();
                        }
                        else
                        {
                            _jsonClient.GetAsync(new DiscountDelete { DiscountId = CurrentDiscount.Id })
                            .Success(r =>
                            {
                                _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
                                r.Message.Message = string.Format(r.Message.Message, CurrentDiscount.Name);
                                Discounts.Remove(Discounts.CurrentItem);
                                _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
                                //_newUserCommand.RaiseCanExecuteChanged();
                            })
                            .Error(ex =>
                            {
                                throw ex;
                            });
                        }
                    }
                });
        }

        private void NewDiscount(object obj)
        {
            CurrentDiscount = new Discount();
        }
    }
}
