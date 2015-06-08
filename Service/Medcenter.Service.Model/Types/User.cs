using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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

        public List<ErrorMessage> Validate()
        {
            List<ErrorMessage> em = new List<ErrorMessage>();
            if (string.IsNullOrEmpty(FirstName)) em.Add(new ErrorMessage("Имя, отчество:",ErrorMessages.EmptyString));
            if (string.IsNullOrEmpty(LastName)) em.Add(new ErrorMessage("Фамилия:", ErrorMessages.EmptyString));
            if (string.IsNullOrEmpty(DisplayName)) em.Add(new ErrorMessage("Отображаемое имя:", ErrorMessages.EmptyString));
            if (UserId == 0 && string.IsNullOrEmpty(Password)) em.Add(new ErrorMessage("Пароль:", ErrorMessages.EmptyString));
            if (UserId == 0 && string.IsNullOrEmpty(Password1)) em.Add(new ErrorMessage("Повтор пароля:", ErrorMessages.EmptyString));
            if (string.IsNullOrEmpty(UserName)) em.Add(new ErrorMessage("Логин:", ErrorMessages.EmptyString));
            if (!string.IsNullOrEmpty(Password) && Password != Password1) em.Add(new ErrorMessage("Пароль:", ErrorMessages.PasswordMismatch));
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
