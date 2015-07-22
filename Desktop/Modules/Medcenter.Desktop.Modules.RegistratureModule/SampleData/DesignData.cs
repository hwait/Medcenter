﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Desktop.Modules.RegistratureModule.Model;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.RegistratureModule.SampleData
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

        #region Doctors

        private static Doctor Bacula
        {
            get
            {
                return new Doctor
                {
                    Id = 1,
                    Name = "Бацула Людмила Пантелеевна",
                    ShortName = "Бацула Л.П.",
                    Color = Colors.Bisque.ToString()
                };
            }
        }

        private static Doctor VVA
        {
            get
            {
                return new Doctor
                {
                    Id = 2,
                    Name = "Вдовиченко Владимир Андреевич",
                    ShortName = "Вдовиченко В.А.",
                    Color = Colors.LightSkyBlue.ToString()
                };
            }
        }

        private static Doctor LGV
        {
            get
            {
                return new Doctor
                {
                    Id = 3,
                    Name = "Лосева Галина Викторовна",
                    ShortName = "Лосева Г.В.",
                    Color = Colors.LightGreen.ToString()
                };
            }
        }

        #endregion

        #region Schedules

        private static Schedule _schedule1
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 7, 30, 0),
                    End = new DateTime(2015, 7, 11, 11, 55, 0)
                };
            }
        }

        private static Schedule _schedule2
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 12, 30, 0),
                    End = new DateTime(2015, 7, 11, 17, 55, 0)
                };
            }
        }

        private static Schedule _schedule3
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor = LGV,
                    Start = new DateTime(2015, 7, 11, 12, 30, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                };
            }
        }

        #endregion

        #region Receptions 1

        private Reception _reception11
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 0,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0),
                    Duration = 90,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        private Reception _reception12
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 1,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0),
                    Duration = 15,
                    Packages = _packages4,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception13
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 2,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 15, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 2,
                    Payment = null
                };
            }
        }

        private Reception _reception14
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 0,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 20, 0),
                    Duration = 60,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        private Reception _reception15
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 20, 0),
                    Duration = 10,
                    Packages = _packages2,
                    Status = 3,
                    Payment = null
                };
            }
        }

        private Reception _reception16
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0),
                    Duration = 85,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        #endregion
        #region Receptions 2

        private Reception _reception21
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 0,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0),
                    Duration = 40,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        private Reception _reception22
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 1,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                    Duration = 25,
                    Packages = _packages1,
                    Status = 2,
                    Payment = null
                };
            }
        }

        private Reception _reception23
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 2,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 35, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 3,
                    Payment = null
                };
            }
        }

        private Reception _reception24
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 0,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 40, 0),
                    Duration = 10,
                    Packages = _packages2,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception25
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 50, 0),
                    Duration = 20,
                    Packages = _packages4,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception26
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 10, 0),
                    Duration = 225,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        #endregion
        #region Receptions 3

        private Reception _reception31
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 1,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0),
                    Duration = 20,
                    Packages = _packages3,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception32
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 1,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 50, 0),
                    Duration = 15,
                    Packages = _packages1,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception33
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 2,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 05, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 3,
                    Payment = null
                };
            }
        }

        private Reception _reception34
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 0,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                    Duration = 10,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }

        private Reception _reception35
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 20, 0),
                    Duration = 15,
                    Packages = _packages1,
                    Status = 1,
                    Payment = null
                };
            }
        }

        private Reception _reception36
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13,35, 0),
                    Duration = 55,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }
        private Reception _reception37
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 30, 0),
                    Duration = 15,
                    Packages = _packages2,
                    Status = 2,
                    Payment = null
                };
            }
        }
        private Reception _reception38
        {
            get
            {
                return new Reception
                {
                    Id = 0,
                    PatientId = 3,
                    Schedule = _schedule1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 45, 0),
                    Duration = 15,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    Payment = null
                };
            }
        }
        #endregion

        public Reception CurrentReception
        {
            get
            {
                return _reception22;
            }
        }

        #region Package

        private Package _cardio
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

        private Package _cardiologist
        {
            get
            {
                return new Package
                {
                    Id = 0,
                    Name = "Консультация кардиолога",
                    ShortName = "Кардиолог",
                    Duration = 5,
                    DoctorId = 0,
                    Cost = 1200
                };
            }
        }

        private Package _kidneys
        {
            get
            {
                return new Package
                {
                    Id = 0,
                    Name = "УЗИ почек",
                    ShortName = "Почки",
                    Duration = 10,
                    DoctorId = 1,
                    Cost = 2100
                };
            }
        }
        private Package _kidneysDoppler
        {
            get
            {
                return new Package
                {
                    Id = 0,
                    Name = "УЗИ почек с допплерографией",
                    ShortName = "УЗДГ почек",
                    Duration = 5,
                    DoctorId = 1,
                    Cost = 3500
                };
            }
        }
        #endregion

        #region Packages

        private ObservableCollection<Package> _packages1
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    _cardio,
                    _cardiologist,
                    _kidneys,
                    _kidneysDoppler
                };
            }
        }

        private ObservableCollection<Package> _packages2
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    _kidneys,
                    _kidneysDoppler
                };
            }
        }

        private ObservableCollection<Package> _packages3
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    _kidneysDoppler
                };
            }
        }

        private ObservableCollection<Package> _packages4
        {
            get
            {
                return new ObservableCollection<Package>
                {
                    _cardio,
                    _cardiologist
                };
            }
        }

        #endregion

        public ObservableCollection<CabinetReceptions> DayReceptions
        {
            get
            {
                return new ObservableCollection<CabinetReceptions>
                {
                    new CabinetReceptions
                    {
                        CabinetId = "1",
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception11,
                            _reception12,
                            _reception13,
                            _reception14,
                            _reception15
                        }
                    },
                    new CabinetReceptions
                    {
                        CabinetId = "1",
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception21,
                            _reception22,
                            _reception23,
                            _reception24,
                            _reception25
                        }
                    },
                    new CabinetReceptions
                    {
                        CabinetId = "2",
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception31,
                            _reception32,
                            _reception33,
                            _reception34,
                            _reception35,
                            _reception36,
                            _reception37
                        }
                    }
                };
            }
        }
    }
}
