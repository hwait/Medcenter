using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Medcenter.Desktop.Infrastructure;
using Medcenter.Service.Model.Messaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Medcenter.Desktop.Modules.StatusbarModule.ViewModels
{
     [Export]
     public class StatusbarViewModel : BindableBase
    {
         private readonly IEventAggregator _eventAggregator;
         private readonly DelegateCommand<object> _showMessagesListCommand;
         public ICommand ShowMessagesListCommand
         {
             get { return this._showMessagesListCommand; }
         }
         public bool IsMessagesListShow
         {
             get { return _isMessagesListShow; }
             set
             {
                 SetProperty(ref _isMessagesListShow, value);
             }
         }

         public ObservableCollection<ResultMessage> StatusMessages
         {
             get
             {
                 return _statusMessages;
             }
             set
             {
                 SetProperty(ref _statusMessages, value);
             }
         }
         ObservableCollection<ResultMessage> _statusMessages;
         public ResultMessage AggregateMessage
         {
             get
             {
                 return _aggregateMessage;
             }
             set
             {
                 SetProperty(ref _aggregateMessage, value);
                 if (value!=null) _dispatcherTimer.Start();
             }
         }
         private bool _busyIndicator;
         public bool BusyIndicator
         {
             get { return _busyIndicator; }
             set { SetProperty(ref _busyIndicator, value); }
         }
         private bool _statusButtonIndicator;
         public bool StatusButtonIndicator
         {
             get { return _statusButtonIndicator; }
             set { SetProperty(ref _statusButtonIndicator, value); }
         }
         private ResultMessage _aggregateMessage;
         private bool _isMessagesListShow;
         DispatcherTimer _dispatcherTimer = new DispatcherTimer();
         [ImportingConstructor]
         public StatusbarViewModel(IEventAggregator eventAggregator)
         {
             IsMessagesListShow = false;
             _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
             _dispatcherTimer.Interval = new TimeSpan(0, 0, Utils.TimerShowMessage);
             _showMessagesListCommand = new DelegateCommand<object>(ShowMessagesList);
             _eventAggregator = eventAggregator;
             _eventAggregator.GetEvent<OperationResultEvent>().Subscribe(OperationResultGet);
             _eventAggregator.GetEvent<IsBusyEvent>().Subscribe(ToggleBusyIndicator);
             _statusMessages = new ObservableCollection<ResultMessage>();
         }

         private void ToggleBusyIndicator(bool obj)
         {
             BusyIndicator = obj;
             StatusButtonIndicator = !obj;
         }

         private void ShowMessagesList(object obj)
         {
             IsMessagesListShow = !IsMessagesListShow;
         }

         private void OperationResultGet(ResultMessage msg)
         {
             StatusMessages.Add(msg);
             AggregateMessage = msg;
         }
         private void DispatcherTimer_Tick(object sender, EventArgs e)
         {
             AggregateMessage = null;
             (sender as DispatcherTimer).Stop();
         }
    }
}
