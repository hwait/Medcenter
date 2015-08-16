using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.CabinetModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.CabinetModule.Views
{
    [Export("CabinetMainView")]
    public partial class CabinetMainView : UserControl, IView
    {
        public CabinetMainView()
        {
            InitializeComponent();
        }
        [Import]
        public CabinetMainViewModel ViewModel
        {
            get { return (CabinetMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
