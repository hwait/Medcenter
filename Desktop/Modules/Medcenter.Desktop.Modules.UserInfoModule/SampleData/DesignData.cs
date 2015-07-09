using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.UserInfoModule.SampleData
{
    public class DesignData
    {
        public User CurrentUser
        {
            get
            {
                return new User
                {
                    DisplayName = "Бацула Л.П.",
                    FirstName = "Людмила Пантелеевна",
                    LastName = "Бацула",
                    Roles = new ObservableCollection<string>
                    {
                        "Admin",
                        "Doctor"
                    },
                    Permissions = new ObservableCollection<string>
                    {
                        "CanEditMainLists",
                        "CanGetDailyBalance"
                    },
                    UserName = "bacula"
                };
            }
        }
        public List<ResultMessage> Errors
        {
            get
            {
                return new List<ResultMessage>
            {
                new ResultMessage(2,"Фамилия:",OperationErrors.EmptyString),
                new ResultMessage(2,"Пароль:",OperationErrors.PasswordMismatch)
            };
            }
        }
        private readonly ObservableCollection<User> _usersFiltered = new ObservableCollection<User>  
        {
            new User
            {
                DisplayName = "Бацула Л.П.",
                FirstName = "Людмила Пантелеевна",
                LastName = "Бацула",
                Roles = new ObservableCollection<string>
                {
                    "Admin",
                    "Doctor"
                },
                Permissions = new ObservableCollection<string>
                {
                    "CanEditMainLists",
                    "CanGetDailyBalance"
                },
                UserName = "bacula"
            },
            new User
            {
                DisplayName = "Кашавкина Н.П.",
                FirstName = "Нина Павловна",
                LastName = "Кашавкина",
                Roles = new ObservableCollection<string>
                {
                    "Manager",
                    "Nurse"
                },
                Permissions = new ObservableCollection<string>
                {
                    "CanEditMainLists",
                    "CanGetDailyBalance"
                },
                UserName = "kanike"
            },
            new User
            {
                DisplayName = "Вдовиченко В.А.",
                FirstName = "Владимир Андреевич",
                LastName = "Вдовиченко",
                Roles = new ObservableCollection<string>
                {
                    "Owner",
                    "Doctor",
                    "Admin"
                },
                Permissions = new ObservableCollection<string>
                {
                    "CanEditMainLists",
                    "CanGetFinancialInfo"
                },
                UserName = "vva"
            },
            new User
            {
                DisplayName = "Лосева Г.В.",
                FirstName = "Галина Викторовна",
                LastName = "Лосева",
                Roles = new ObservableCollection<string>
                {
                    "Owner",
                    "Doctor"
                },
                Permissions = new ObservableCollection<string>(),
                UserName = "loseva"
            }
        };

        public RolesCollection RolesDictionary
        {
            get { return new RolesCollection(new ObservableCollection<string>()); }
        }
        public PermissionsCollection PermissionsDictionary
        {
            get { return new PermissionsCollection(new ObservableCollection<string>()); }
        }
        public ICollectionView UsersFiltered
        {
            get
            {
                return new CollectionView(_usersFiltered);
            }
        }
    }
}
