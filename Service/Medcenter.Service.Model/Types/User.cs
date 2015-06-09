using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using ServiceStack.Auth;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class User
    {
        public User()
        {
            Roles = new ObservableCollection<string>();
            Permissions=new ObservableCollection<string>();
        }
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public ObservableCollection<string> Roles { get; set; }
        [DataMember]
        public ObservableCollection<string> Permissions { get; set; }
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string Password { get; set; }
        public string Password1 { get; set; }
        public string RolesString {
            get
            {
                var roles = new RolesCollection(Roles);
                return string.Join(", ", roles.RolesValues);
            }
        }

        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(FirstName)) em.Add(new ResultMessage(2,"Имя, отчество:",OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(LastName)) em.Add(new ResultMessage(2, "Фамилия:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(DisplayName)) em.Add(new ResultMessage(2, "Отображаемое имя:", OperationErrors.EmptyString));
            if (UserId == 0 && string.IsNullOrEmpty(Password)) em.Add(new ResultMessage(2, "Пароль:", OperationErrors.EmptyString));
            if (UserId == 0 && string.IsNullOrEmpty(Password1)) em.Add(new ResultMessage(2, "Повтор пароля:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(UserName)) em.Add(new ResultMessage(2, "Логин:", OperationErrors.EmptyString));
            if (!string.IsNullOrEmpty(Password) && Password != Password1) em.Add(new ResultMessage(2, "Пароль:", OperationErrors.PasswordMismatch));
            return em;
        }

        public void ClearAll()
        {
            UserId = 0;
            UserName = "";
            FirstName = "";
            LastName = "";
            DisplayName = "";
            SessionId = "";
            Password = "";
            Password1 = "";
            Roles=new ObservableCollection<string>();
            Permissions = new ObservableCollection<string>();
        }
        public void ClearPassword()
        {
            Password = "";
            Password1 = "";
        }
    }
}
