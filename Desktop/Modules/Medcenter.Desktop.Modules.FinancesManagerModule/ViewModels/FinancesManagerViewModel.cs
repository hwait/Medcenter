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
        private readonly DelegateCommand<object> _discountFotoChooseCommand;
        private readonly DelegateCommand<object> _saveDiscountCommand;
        private readonly DelegateCommand<object> _discountEditCommand;
        private readonly DelegateCommand<object> _discountReportsCommand;
        #region Properties

        public ICommand DiscountFotoChooseCommand
        {
            get { return this._discountFotoChooseCommand; }
        }

        public ICommand SaveDiscountCommand
        {
            get { return this._saveDiscountCommand; }
        }
        public ICommand DiscountReportsCommand
        {
            get { return this._discountReportsCommand; }
        }
        public ICommand DiscountEditCommand
        {
            get { return this._discountEditCommand; }
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
        private Discount _currentDiscount;

        public Discount CurrentDiscount
        {
            get { return _currentDiscount; }
            set
            {
                SetProperty(ref _currentDiscount, value);
            }
        }
        private bool _isDiscountEdit;
        public bool IsDiscountEdit
        {
            get { return _isDiscountEdit; }
            set { SetProperty(ref _isDiscountEdit, value); }
        }
        private bool _isDiscountReports;
        public bool IsDiscountReports
        {
            get { return _isDiscountReports; }
            set { SetProperty(ref _isDiscountReports, value); }
        }
        #endregion

        [ImportingConstructor]
        public FinancesManagerViewModel(IRegionManager regionManager, JsonServiceClient jsonClient, IEventAggregator eventAggregator)
        {

            _regionManager = regionManager;
            _jsonClient = jsonClient;
            _eventAggregator = eventAggregator;
            _discountFotoChooseCommand = new DelegateCommand<object>(DiscountFotoChoose);
            _saveDiscountCommand = new DelegateCommand<object>(SaveDiscount);
            _discountEditCommand = new DelegateCommand<object>(DiscountEdit);
            _discountReportsCommand = new DelegateCommand<object>(DiscountReports);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            //_eventAggregator.GetEvent<FinancesManagerEvent>().Subscribe(FinancesManagerReceived);
            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            IsDiscountEdit = false;
            IsDiscountReports = false;

        }

        //private void FinancesManagerReceived(Discount obj)
        //{
        //    CurrentDiscount = obj;
        //    ShowDiscountFoto(CurrentDiscount.DiscountId);
        //}

        private void DiscountReports(object obj)
        {
            IsDiscountEdit = false;
            IsDiscountReports = true;
        }

        private void DiscountEdit(object obj)
        {
            IsDiscountEdit = true;
            IsDiscountReports = false;
        }

        private void SaveDiscount(object obj)
        {
            //bool isNew = CurrentDiscount.DiscountId == 0;
            Errors = CurrentDiscount.Validate();
            //if (Errors.Count == 0)
            //{
            //    _eventAggregator.GetEvent<IsBusyEvent>().Publish(true);
            //    _jsonClient.PostAsync(new DiscountUpdateInfo { Discount = CurrentDiscount })
            //        .Success(r =>
            //        {
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //            CurrentDiscount.DiscountId = r.DiscountId;
            //            CurrentDiscount.Password = "";
            //            CurrentDiscount.Password1 = "";
            //            r.Message.Message = string.Format(r.Message.Message, CurrentDiscount.DisplayName);
            //            _eventAggregator.GetEvent<OperationResultEvent>().Publish(r.Message);
            //            _eventAggregator.GetEvent<FinancesManagerEvent>().Publish(CurrentDiscount);
            //        })
            //        .Error(ex =>
            //        {
            //            _eventAggregator.GetEvent<OperationResultEvent>().Publish(OperationErrors.GetErrorFromText("Сохранение изменений:", ex.Message));
            //            _eventAggregator.GetEvent<IsBusyEvent>().Publish(false);
            //        });
            //}
        }
        private void DiscountFotoChoose(object obj)
        {

        }
    }
}
