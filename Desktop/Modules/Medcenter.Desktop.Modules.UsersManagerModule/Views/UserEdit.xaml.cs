using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Medcenter.Desktop.Modules.UsersManagerModule.ViewModels;

namespace Medcenter.Desktop.Modules.UsersManagerModule.Views
{
    /// <summary>
    /// Interaction logic for UserEdit.xaml
    /// </summary>
    public partial class UserEdit : UserControl
    {
        public UserEdit()
        {
            InitializeComponent();
        }
        [Import]
        public UserEditViewModel ViewModel
        {
            get { return (UserEditViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
