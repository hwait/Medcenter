using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.ModuleTemplate1.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.ModuleTemplate1.Views
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
