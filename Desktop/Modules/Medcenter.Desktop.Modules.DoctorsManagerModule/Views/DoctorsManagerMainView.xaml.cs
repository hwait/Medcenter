using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.DoctorsManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.DoctorsManagerModule.Views
{
    [Export("DoctorsManagerMainView")]
    public partial class DoctorsManagerMainView : UserControl, IView
    {
        public DoctorsManagerMainView()
        {
            InitializeComponent();
        }
        [Import]
        public DoctorsManagerMainViewModel ViewModel
        {
            get { return (DoctorsManagerMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
