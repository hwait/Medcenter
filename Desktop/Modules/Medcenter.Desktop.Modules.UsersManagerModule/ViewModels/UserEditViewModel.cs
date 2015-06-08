using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.UsersManagerModule.ViewModels
{
    [Export]
    public class UserEditViewModel : BindableBase
    {
        public UserEditViewModel(User currentUser)
        {
            _currentUser = currentUser;
        }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }
    }
}
