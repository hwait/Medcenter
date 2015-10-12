using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;
using ServiceStack;
using ServiceStack.Web;

namespace Medcenter.Service.Model.Operations
{
    [Route("/users/{UserId}/foto", "POST")]
    public class UserFotoUpload : IReturn<UserFotoUploadResponse>
    {
        public int UserId { get; set; }
    }
    public class UserFotoUploadResponse : IHasResponseStatus
    {
        public UserFotoUploadResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/users/{UserId}/foto", "GET")]
    public class UserFotoDownload : IReturn<UserFotoDownloadResponse>
    {
        public int UserId { get; set; }
    }
    public class UserFotoDownloadResponse : IHasResponseStatus
    {
        public UserFotoDownloadResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public byte[] FotoStream { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
    [Route("/users/logins", "GET")]
    public class LoginsSelect : IReturn<LoginsSelectResponse>
    {

    }
    public class LoginsSelectResponse : IHasResponseStatus
    {
        public LoginsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<string> Users { get; set; }

    }
    [Authenticate]
    [Route("/users/roles/{DeviceId}", "GET")]
    public class RolesSelect : IReturn<RolesSelectResponse>
    {
        public string DeviceId { get; set; }
    }
    public class RolesSelectResponse : IHasResponseStatus
    {
        public RolesSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public List<string> Roles { get; set; }
    }
    [Route("/users/byrole/{Role}", "GET")]
    public class UsersByRoleSelect : IReturn<UsersByRoleSelectResponse>
    {
        public string Role { get; set; }
    }
    public class UsersByRoleSelectResponse : IHasResponseStatus
    {
        public UsersByRoleSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public List<User> Agents { get; set; }
    }
    [Authenticate]
    [Route("/users/permissions", "GET")]
    public class PermissionsSelect : IReturn<PermissionsSelectResponse>
    {
    }
    public class PermissionsSelectResponse : IHasResponseStatus
    {
        public PermissionsSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResponseStatus ResponseStatus { get; set; }
        public List<string> Permissions { get; set; }
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
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public ObservableCollection<User> Users { get; set; }

    }
    [Route("/user/{UserId}", "GET")]
    public class UserSelect : IReturn<UserSelectResponse>
    {
        public int UserId { get; set; }
    }
    public class UserSelectResponse : IHasResponseStatus
    {
        public UserSelectResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public User User { get; set; }

    }
    [RequiredRole("Admin")]
    [Route("/users/save", "POST")]
    public class UserSave : IReturn<UserSaveResponse>
    {
        public User User { get; set; }
    }
    [Route("/users/update", "POST")]
    public class UserUpdateInfo : IReturn<UserSaveResponse>
    {
        public User User { get; set; }
    }
    public class UserSaveResponse : IHasResponseStatus
    {
        public UserSaveResponse()
        {
            ResponseStatus = new ResponseStatus();
        }
        public ResultMessage Message { get; set; }
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
        public ResultMessage Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
