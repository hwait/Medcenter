using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.DoctorsManagerModule.SampleData
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
                    new ResultMessage(2, "Фамилия:", OperationErrors.EmptyString),
                    new ResultMessage(2, "Пароль:", OperationErrors.PasswordMismatch)
                };
            }
        }

        public Doctor CurrentDoctor
        {
            get
            {
                return new Doctor
                {
                    Id = 0,
                    Name = "Бацула Людмила Пантелеевна",
                    ShortName = "Бацула Л.П.",
                    Color = Colors.Bisque.ToString()
                };
            }
        }

        public Package CurrentPackage
        {
            get
            {
                return new Package
                {
                    Id = 0,
                    Name = "Ультразвуковое исследование сердца",
                    ShortName = "ЭХО",
                    Duration = 10,
                    DoctorId = 0,
                    Cost = 1000
                };
            }
        }

        public ObservableCollection<Doctor> Doctors
        {
            get
            {
                return new ObservableCollection<Doctor>
                {
                    new Doctor
                    {
                        Id = 0,
                        Name = "Бацула Людмила Пантелеевна",
                        ShortName = "Бацула Л.П.",
                        Color = Colors.Bisque.ToString()
                    },
                    new Doctor
                    {
                        Id = 0,
                        Name = "Вдовиченко Владимир Андреевич",
                        ShortName = "Вдовиченко В.А.",
                        Color = Colors.LightSkyBlue.ToString()
                    },
                    new Doctor
                    {
                        Id = 0,
                        Name = "Лосева Галина Викторовна",
                        ShortName = "Лосева Г.В.",
                        Color = Colors.LightGreen.ToString()
                    }
                };
            }
        }

        public ObservableCollection<Package> Packages
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    new Package
                    {
                        Id = 0,
                        Name = "Ультразвуковое исследование сердца",
                        ShortName = "ЭХО",
                        Duration = 10,
                        DoctorId = 0,
                        Cost = 1000
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "Кардиолог",
                        Duration = 5,
                        DoctorId = 0,
                        Cost = 1200
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек с допплерографией",
                        ShortName = "УЗДГ почек",
                        Duration = 5,
                        DoctorId = 1,
                        Cost = 3500
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "Почки",
                        Duration = 10,
                        DoctorId = 1,
                        Cost = 2100
                    },
                };
            }
        }
        public ObservableCollection<Package> PackagesInDoctor
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    new Package
                    {
                        Id = 0,
                        Name = "Ультразвуковое исследование сердца",
                        ShortName = "ЭХО",
                        Duration = 10,
                        DoctorId = 0,
                        Cost = 1000
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "Кардиолог",
                        Duration = 5,
                        DoctorId = 0,
                        Cost = 1200
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек с допплерографией",
                        ShortName = "УЗДГ почек",
                        Duration = 5,
                        DoctorId = 1,
                        Cost = 3500
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "Почки",
                        Duration = 10,
                        DoctorId = 1,
                        Cost = 2100
                    },
                };
            }
        }
    }
}
