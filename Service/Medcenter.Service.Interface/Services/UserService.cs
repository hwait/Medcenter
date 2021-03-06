﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
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
        public LoginsSelectResponse Get(LoginsSelect req)
        {
            List<string> users=new List<string>();
            ResponseStatus status=new ResponseStatus();
            try
            {
                var rows = Db.SqlList<string>("EXEC sp_UsersNames_Select");
                //Db.SqlList<CustomerDTO>("EXEC sp_getcustomers @Name", new { request.Name });
                users = rows.ToList();
            }
            catch (Exception e)
            {
                Logger.Log("LoginsSelectResponse", e);
            }
            return new LoginsSelectResponse { Users = new ObservableCollection<string>(users) };
        }
        
        public RolesSelectResponse Get(RolesSelect req)
        {
            IAuthSession session = GetSession();
            var id = Db.Single<int>("EXEC sp_UserSessions_Login @DeviceId, @SessionId, @UserId", new
            {
                DeviceId = req.DeviceId,
                SessionId = session.Id,
                UserId = session.UserAuthId
            });

            return new RolesSelectResponse { Roles = base.GetSession().Roles };
        }
        public PermissionsSelectResponse Get(PermissionsSelect req)
        {
            IAuthSession session = GetSession();
            return new PermissionsSelectResponse { Permissions = base.GetSession().Permissions };
        }
        public UsersSelectResponse Get(UsersSelect req)
        {
            var rows = Db.SqlList<User>("EXEC sp_Users_Select");
            //Db.SqlList<CustomerDTO>("EXEC sp_getcustomers @Name", new { request.Name });
            //var users = rows.ToList();

            return new UsersSelectResponse { Users = new ObservableCollection<User>(rows) };
        }
        public UsersByRoleSelectResponse Get(UsersByRoleSelect req)
        {
            var rows = Db.SqlList<User>(string.Format("select Id as UserId, DisplayName from UserAuth where Roles like '%{0}%'", req.Role));
            return new UsersByRoleSelectResponse { Agents = rows };
        }
        public UserSelectResponse Get(UserSelect req)
        {
            List<User> users;
            try
            {
                var rows = Db.SqlList<User>("EXEC sp_User_Select @uid", new
                {
                    uid = req.UserId
                });
                users = rows.ToList();
            }
            catch (Exception e)
            {
                Logger.Log("UserSelectResponse", e);
                throw;
            }
            return new UserSelectResponse { User =users[0] };
        }

        public UserSaveResponse Post(UserUpdateInfo req)
        {
            int uid = 0;
            ResultMessage _message;
            IUserAuth user;
            IUserAuthRepository rep = base.TryResolve<IUserAuthRepository>();
            try
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
                    rep.UpdateUserAuth(user, user, req.User.Password);
                }
                uid = user.Id;
                _message = new ResultMessage(0, "Сервис", OperationResults.UserSave);
                Logger.Log("UserUpdateInfo.Saving");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.UserSave);
                Logger.Log("UserUpdateInfo.Saving", e);
                throw;
            }
            return new UserSaveResponse
            {
                UserId = user.Id,
                Message = _message
            };
        }

        public UserSaveResponse Post(UserSave req)
        {
            int uid = 0;
            ResultMessage _message;
            IUserAuth user;
            IUserAuthRepository rep = base.TryResolve<IUserAuthRepository>();
            if (req.User.UserId > 0) // User exists so we're saving 
            {
                try
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
                        rep.UpdateUserAuth(user, user, req.User.Password);
                    }
                    uid = user.Id;
                    _message = new ResultMessage(0, "Сервис", OperationResults.UserSave);
                    Logger.Log("UserSaveResponse.Saving");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.UserSave);
                    Logger.Log("UserSaveResponse.Saving", e);
                    throw;
                }
            }
            else //New user
            {
                try
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
                    uid = user.Id;
                    _message = new ResultMessage(0, "Сервис", OperationResults.UserCreate);
                    Logger.Log("UserSaveResponse.NewUser");
                }
                catch (Exception e)
                {
                    _message = new ResultMessage(2, e.Source, OperationErrors.UserCreate);
                    Logger.Log("UserSaveResponse.NewUser", e);
                    throw;
                }
                
            }
            
            return new UserSaveResponse
            {
                UserId = user.Id,
                Message=_message
            };
        }
        public UserDeleteResponse Get(UserDelete  req)
        {
            ResultMessage _message;
            try
            {
                var res = Db.SqlList<int>("EXEC sp_User_Delete @uid", new
                {
                    uid = req.Id
                });
                _message = new ResultMessage(0, "Сервис", OperationResults.UserDelete);
                Logger.Log("UserDeleteResponse");
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, e.Source, OperationErrors.UserDelete);
                Logger.Log("UserDeleteResponse", e);
                throw;
            }
            return new UserDeleteResponse { Message = _message };
        }

        public UserFotoUploadResponse Post(UserFotoUpload request)
        {
            ResultMessage _message;
            string curop = "Загрузка фото";
            string path = GetFotosPath(request.UserName);
            if (Request.Files == null || Request.Files.Length == 0)
                _message = new ResultMessage(1, curop, OperationErrors.UserFotoIsMissing);

            // Save the file
            try
            {
                Request.Files[0].SaveTo(path);
                _message = new ResultMessage(0, curop, OperationResults.UserFotoUpload);
            }
            catch (Exception e)
            {
                _message = new ResultMessage(2, curop, OperationErrors.UserFileSave);
                Logger.Log("UserFotoUploadResponse", e);
                throw;
            }
            return new UserFotoUploadResponse { Message = _message };
        }

        private string GetFotosPath(string userName)
        {
            return string.Format("{0}Resources\\Fotos\\{1}.jpg", AppDomain.CurrentDomain.BaseDirectory, userName);
        }

        public UserFotoDownloadResponse Get(UserFotoDownload request)
        {
            byte[] fileStream;
            try
            {
                string path = GetFotosPath(request.UserName);
                if (File.Exists(path))
                {
                    fileStream = File.ReadAllBytes(path);
                }
                else
                {
                    fileStream = null;
                }
            }
            catch (Exception e)
            {
                Logger.Log("UserFotoDownloadResponse", e);
                throw;
            }

            return new UserFotoDownloadResponse { FotoStream=fileStream };
        }
    }
}
