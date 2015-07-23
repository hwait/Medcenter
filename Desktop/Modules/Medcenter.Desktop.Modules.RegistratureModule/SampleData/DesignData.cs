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

        public bool IsNewPatientPanelVisible = true;
        public bool IsSearchPatientPanelVisible = true;
        public bool IsAddPackagesPanelVisible = true;
        public bool IsReceptionPanelVisible = true;
        public bool IsPaymentsPanelVisible = true;

        public string PatientSearchString = "Петров";
        #region Doctors
        private static Doctor GrayDoctor
        {
            get
            {
                return new Doctor
                {
                    Id = 0,
                    Name = "",
                    ShortName = "",
                    Color = Colors.LightGray.ToString()
                };
            }
        }
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
        private static Schedule _scheduleFake1
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = GrayDoctor,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 7, 30, 0)
                };
            }
        }
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
        private static Schedule _scheduleFake2
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = GrayDoctor,
                    Start = new DateTime(2015, 7, 11, 11, 55, 0),
                    End = new DateTime(2015, 7, 11, 12, 30, 0)
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
        private static Schedule _scheduleFake3
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = GrayDoctor,
                    Start = new DateTime(2015, 7, 11, 17, 55, 0),
                    End = new DateTime(2015, 7, 11, 19, 0, 0)
                };
            }
        }
        private static Schedule _scheduleFake4
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor = GrayDoctor,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 30, 0)
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
        private static Schedule _scheduleFake5
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor = GrayDoctor,
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 19, 0, 0)
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
        private List<Reception> _receptions1
        {
            get
            {
                return new List<Reception>
                {
                    _reception31,_reception32,_reception33,_reception35
                };
            }
        }
        private List<Reception> _receptions2
        {
            get
            {
                return new List<Reception>
                {
                    _reception22,_reception23,_reception24,_reception25
                };
            }
        }
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
        public ObservableCollection<Package> Packages
        {
            get
            {
                return _packages1;
            }
        }

        #region ScheduleReceptions

        private ObservableCollection<ScheduleReception> _scheduleReceptions1
        {
            get
            {
                return new ObservableCollection<ScheduleReception>
                {
                    new ScheduleReception
                    {
                        Schedule = _scheduleFake1,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule = _schedule1,
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception11,
                            _reception12,
                            _reception13,
                            _reception14,
                            _reception15,
                            _reception16
                        }
                    },
                    new ScheduleReception
                    {
                        Schedule = _scheduleFake2,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule = _schedule2,
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception21,
                            _reception22,
                            _reception23,
                            _reception24,
                            _reception25,
                            _reception26
                        }
                    },
                    new ScheduleReception
                    {
                        Schedule = _scheduleFake3,
                        Receptions = new ObservableCollection<Reception>()
                    }
                };
            }
        }

        private ObservableCollection<ScheduleReception> _scheduleReceptions2
        {
            get
            {
                return new ObservableCollection<ScheduleReception>
                {
                    new ScheduleReception
                    {
                        Schedule = _scheduleFake4,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule = _schedule3,
                        Receptions = new ObservableCollection<Reception>
                        {
                            _reception31,
                            _reception32,
                            _reception33,
                            _reception34,
                            _reception35,
                            _reception36,
                            _reception37,
                            _reception38
                        }
                    },
                    new ScheduleReception
                    {
                        Schedule = _scheduleFake5,
                        Receptions = new ObservableCollection<Reception>()
                    },
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
                        CabinetId = "Каб. 1",
                        ScheduleReceptions = _scheduleReceptions1
                    },
                    new CabinetReceptions
                    {
                        CabinetId = "Каб. 2",
                        ScheduleReceptions = _scheduleReceptions2
                    }
                };
            }
        }

        #region Contacts

        private Dictionary<string, string> _contacts1
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"E-mail", "patient1@mail.ru"},
                    {"VK", "patient1@mail.ru"},
                    {"Skype", "patient1"}
                };
            }
        }

        private Dictionary<string, string> _contacts2
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"E-mail", "patient2@mail.ru"},
                    {"VK", "patient2@mail.ru"},
                    {"Skype", "patient2"}
                };
            }
        }

        private Dictionary<string, string> _contacts3
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"E-mail", "patient3@mail.ru"},
                    {"VK", "patient3@mail.ru"},
                    {"Skype", "patient3"}
                };
            }
        }

        #endregion

        #region Patients

        private Patient _patient1
        {
            get
            {
                return new Patient
                {
                    Id = 0,
                    Surname = "Петров",
                    FirstName = "Вениамин",
                    SecondName = "Адольфович",
                    BirthDate = new DateTime(1987, 11, 3),
                    Gender = true,
                    CityId = 1,
                    Address = "Брауншвейгская, 23, кв 2",
                    PhoneNumber = "525356",
                    MobileCode = "7705",
                    MobileNumber = "5048521",
                    Contacts = _contacts1,
                    Receptions = _receptions1,
                };
            }
        }

        private Patient _patient2
        {
            get
            {
                return new Patient
                {
                    Id = 0,
                    Surname = "Петров",
                    FirstName = "Аристарх",
                    SecondName = "Иосифович",
                    BirthDate = new DateTime(1947, 1, 23),
                    Gender = true,
                    CityId = 1,
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "7777",
                    MobileNumber = "4568721",
                    Contacts = _contacts2,
                    Receptions = new List<Reception>(),
                };
            }
        }

        private Patient _patient3
        {
            get
            {
                return new Patient
                {
                    Id = 0,
                    Surname = "Петрова",
                    FirstName = "Агриппина",
                    SecondName = "Львовна",
                    BirthDate = new DateTime(1997, 5, 13),
                    Gender = false,
                    CityId = 1,
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "",
                    MobileNumber = "",
                    Contacts = _contacts3,
                    Receptions = _receptions2,
                };
            }
        }
        public List<Patient> Patients
        {
            get
            {
                return new List<Patient>
                {
                    _patient1,_patient2,_patient3
                };
            }
        }
        #endregion

        public Patient CurrentPatient
        {
            get { return _patient1; }
        }
    }
}
