﻿using System;
using System.Collections.Generic;
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
using Medcenter.Desktop.Infrastructure;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;

namespace Medcenter.Desktop.Modules.LoginModule.Views
{
    [Export]
    [ViewSortHint("03")]
    public partial class LogoutToolbarView : UserControl
    {
        [Import]
        private IEventAggregator _eventAggregator;
        public LogoutToolbarView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _eventAggregator.GetEvent<UserLoginEvent>().Publish(null);
        }
    }
}
