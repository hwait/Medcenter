using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.LoginModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.LoginModule.Views
{
     [Export("LoginFormView")]
    public partial class LoginFormView : UserControl, IView
    {
        public LoginFormView()
        {
            InitializeComponent();

        }
        [Import]
        public LoginFormViewModel ViewModel
        {
            get { return (LoginFormViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
