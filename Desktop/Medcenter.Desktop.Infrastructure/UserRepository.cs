using System.ComponentModel.Composition;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Infrastructure
{
    [Export(typeof(IUserRepository))]
    public class UserRepository : IUserRepository
    {
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        
    }
}
