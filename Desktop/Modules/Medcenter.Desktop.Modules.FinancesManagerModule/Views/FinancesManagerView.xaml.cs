using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.FinancesManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.FinancesManagerModule.Views
{
    [Export("FinancesManagerView")]
    public partial class FinancesManagerView : UserControl, IView
    {
        public FinancesManagerView()
        {
            InitializeComponent();
        }
        [Import]
        public FinancesManagerViewModel ViewModel
        {
            get { return (FinancesManagerViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
