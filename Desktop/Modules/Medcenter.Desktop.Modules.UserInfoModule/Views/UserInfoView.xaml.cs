using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.UserInfoModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.UserInfoModule.Views
{
    [Export("UserInfoView")]
    public partial class UserInfoView : UserControl, IView
    {
        public UserInfoView()
        {
            InitializeComponent();
        }
        [Import]
        public UserInfoViewModel ViewModel
        {
            get { return (UserInfoViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
