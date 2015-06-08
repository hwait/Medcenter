using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace Medcenter.Service.Model.Operations
{
    [Route("/users/logins", "GET")]
    public class UserSelect : IReturn<UserSelectResponse>
    {

    }
    public class UserSelectResponse : IHasResponseStatus
    {
        public UserSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<string> Users { get; set; }

    }
    [Authenticate]
    [Route("/users/roles", "GET")]
    public class RolesSelect : IReturn<RolesSelectResponse>
    {

    }
    public class RolesSelectResponse : IHasResponseStatus
    {
        public RolesSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<string> Roles { get; set; }

    }

    [RequiredRole("Admin")]
    [Route("/users/all", "GET")]
    public class UsersSelect : IReturn<UsersSelectResponse>
    {

    }
    public class UsersSelectResponse : IHasResponseStatus
    {
        public UsersSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<User> Users { get; set; }

    }
    [RequiredRole("Admin")]
    [Route("/users/save", "POST")]
    public class UserSave : IReturn<UserSaveResponse>
    {
        public User User { get; set; }
    }
    public class UserSaveResponse : IHasResponseStatus
    {
        public UserSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public int UserId { get; set; }

    }
    [RequiredRole("Admin")]
    [Route("/users/delete/{Id}", "GET")]
    public class UserDelete : IReturn<UserDeleteResponse>
    {
        public int Id { get; set; }
    }
    public class UserDeleteResponse : IHasResponseStatus
    {
        public UserDeleteResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
