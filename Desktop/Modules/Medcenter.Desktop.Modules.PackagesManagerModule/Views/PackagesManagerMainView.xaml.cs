using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.PackagesManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.PackagesManagerModule.Views
{
    [Export("PackagesManagerMainView")]
    public partial class PackagesManagerMainView : UserControl, IView
    {
        public PackagesManagerMainView()
        {
            InitializeComponent();
        }
        [Import]
        public PackagesManagerMainViewModel ViewModel
        {
            get { return (PackagesManagerMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
