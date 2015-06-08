using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Operations;
using Medcenter.Service.Model.Types;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;


namespace Medcenter.Service.Interface.Services
{
    public class UserService : ServiceStack.Service
    {
        //IDbConnectionFactory _db = HostContext.TryResolve<IDbConnectionFactory>();
        //private IDbConnection _db;
        public UserSelectResponse Get(UserSelect req)
        {
           // var db = HostContext.Resolve<IDbConnectionFactory>().Open();

            var rows = Db.SqlList<string>("EXEC sp_UsersNames_select");
            //Db.SqlList<CustomerDTO>("EXEC sp_getcustomers @Name", new { request.Name });
            var users = rows.ToList();

            return new UserSelectResponse { Users = new ObservableCollection<string>(users) };
        }
        public RolesSelectResponse Get(RolesSelect req)
        {
            return new RolesSelectResponse { Roles = new ObservableCollection<string>(base.GetSession().Roles) };
        }
        public UsersSelectResponse Get(UsersSelect req)
        {
            var rows = Db.SqlList<User>("EXEC sp_Users_select");
            //Db.SqlList<CustomerDTO>("EXEC sp_getcustomers @Name", new { request.Name });
            //var users = rows.ToList();

            return new UsersSelectResponse { Users = new ObservableCollection<User>(rows) };
        }
        public UserSaveResponse Post(UserSave req)
        {
            IUserAuth user;
            IUserAuthRepository rep = base.TryResolve<IUserAuthRepository>();
            if (req.User.UserId > 0) // User exists so we're saving 
            {
                user = rep.GetUserAuth(req.User.UserId.ToString());
                user.FirstName = req.User.FirstName;
                user.LastName = req.User.LastName;
                user.DisplayName = req.User.DisplayName;
                user.Roles = req.User.Roles.ToList();
                user.Permissions = req.User.Permissions.ToList();
            
                if (string.IsNullOrEmpty(req.User.Password))
                {
                    rep.SaveUserAuth(user);
                }
                else // Password reset
                {
                    rep.UpdateUserAuth(user,user,req.User.Password);
                }
            }
            else //New user
            {
                user = rep.CreateUserAuth(new UserAuth
                {
                    FirstName = req.User.FirstName,
                    UserName = req.User.UserName,
                    LastName = req.User.LastName,
                    DisplayName = req.User.DisplayName,
                    Roles = req.User.Roles.ToList(),
                    Permissions = req.User.Permissions.ToList()

                }, req.User.Password);
            }
            //string roles = string.Format("[{0}]",string.Join(",", req.User.Roles));
            //string permissions = string.Format("[{0}]",string.Join(",", req.User.Permissions));
            //List<int> res = Db.SqlList<int>("EXEC sp_User_save @uid, @UserName, @FirstName, @LastName, @DisplayName,@Roles,@Permissions", new
            //{
            //    uid=req.User.UserId,
            //    UserName = req.User.UserName,
            //    FirstName=req.User.FirstName,
            //    LastName=req.User.LastName,
            //    DisplayName=req.User.DisplayName,
            //    Roles=roles,
            //    Permissions=permissions
            //});
            return new UserSaveResponse { UserId = user.Id };
        }
       
        public UserDeleteResponse Get(UserDelete  req)
        {
            var res = Db.SqlList<int>("EXEC sp_User_delete @uid", new
            {
                uid = req.Id
            });

            return new UserDeleteResponse {  };
        }
    }
}
