using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.RegistratureModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.RegistratureModule.Views
{
    [Export("RegistratureMainView")]
    public partial class RegistratureMainView : UserControl, IView
    {
        public RegistratureMainView()
        {
            InitializeComponent();
        }
        [Import]
        public RegistratureMainViewModel ViewModel
        {
            get { return (RegistratureMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
