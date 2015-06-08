using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.UsersManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.UsersManagerModule.Views
{
    [Export("UsersManagerMainView")]
    public partial class UsersManagerMainView : UserControl, IView
    {
        public UsersManagerMainView()
        {
            InitializeComponent();
        }
        [Import]
        public UsersManagerMainViewModel ViewModel
        {
            get { return (UsersManagerMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
