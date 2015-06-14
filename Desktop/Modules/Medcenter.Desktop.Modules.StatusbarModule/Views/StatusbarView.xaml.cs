using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.StatusbarModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.StatusbarModule.Views
{
    [Export("StatusbarView")]
    public partial class StatusbarView : UserControl, IView
    {
        public StatusbarView()
        {
            InitializeComponent();
        }
        [Import]
        public StatusbarViewModel ViewModel
        {
            get { return (StatusbarViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
