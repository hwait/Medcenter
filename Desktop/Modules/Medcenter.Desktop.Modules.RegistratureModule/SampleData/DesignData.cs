using System;
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

        #region Discounts
        private static Discount _discount1
        {
            get
            {
                return new Discount
                {
                    Id = 1,
                    Name = "Бесплатно"
                };
            }
        }
        private static Discount _discount2
        {
            get
            {
                return new Discount
                {
                    Id = 1,
                    Name = "Пенсионерам 5"
                };
            }
        }

        private static Discount _discount3
        {
            get
            {
                return new Discount
                {
                    Id = 1,
                    Name = "ВОВ 50"
                };
            }
        }

        public ObservableCollection<Discount> Discounts
        {
            get
            {
                return new ObservableCollection<Discount>() { _discount1, _discount2, _discount3 };
            }
        }

        #endregion

        #region Cities
        private static City _city1
        {
            get
            {
                return new City
                {
                    Id = 1,
                    Name = "Петропавловск",
                    PhoneCode = "77152"
                };
            }
        }
        private static City _city2
        {
            get
            {
                return new City
                {
                    Id = 2,
                    Name = "Петропавловск-Камчатский",
                    PhoneCode = "7885"
                };
            }
        }

        private static City _city3
        {
            get
            {
                return new City
                {
                    Id = 3,
                    Name = "Пресновка",
                    PhoneCode = "77154"
                };
            }
        }

        public ObservableCollection<City> Cities
        {
            get
            {
                return new ObservableCollection<City>() { _city1, _city2, _city3};
            }
        }

        #endregion


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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0),
                    Duration = 90,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0),
                    Duration = 15,
                    Packages = _packages4,
                    Status = 1,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 15, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 2,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 20, 0),
                    Duration = 60,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 20, 0),
                    Duration = 10,
                    Packages = _packages2,
                    Status = 3,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0),
                    Duration = 85,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0),
                    Duration = 40,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
                };
            }
        }
        private Payment _payment1
        {
            get
            {
                return new Payment
                {
                    Id = 0,
                    Cost = 5600,
                    ReceptionId = 7,
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                };
            }
        }
        private Payment _payment2
        {
            get
            {
                return new Payment
                {
                    Id = 0,
                    Cost = 1400,
                    ReceptionId = 7,
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                };
            }
        }
        private Reception _reception22
        {
            get
            {
                return new Reception
                {
                    Id = 7,
                    Payments = new ObservableCollection<Payment> { _payment1, _payment2 },
                    PatientId = 1,
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                    Duration = 25,
                    Packages = _packages1,
                    Status = 2,
                    StatusText = "Идёт исследование"
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
                    ScheduleId=2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 35, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 3,
                    
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
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 40, 0),
                    Duration = 10,
                    Packages = _packages2,
                    Status = 1,
                    
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
                    ScheduleId=2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 50, 0),
                    Duration = 20,
                    Packages = _packages4,
                    Status = 1,
                    
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
                    ScheduleId=2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 10, 0),
                    Duration = 225,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0),
                    Duration = 20,
                    Packages = _packages3,
                    Status = 1,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 50, 0),
                    Duration = 15,
                    Packages = _packages1,
                    Status = 1,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 05, 0),
                    Duration = 5,
                    Packages = _packages3,
                    Status = 3,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                    Duration = 10,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 20, 0),
                    Duration = 15,
                    Packages = _packages1,
                    Status = 1,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13,35, 0),
                    Duration = 55,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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
                    ScheduleId=3,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 30, 0),
                    Duration = 15,
                    Packages = _packages2,
                    Status = 2,
                    
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
                    ScheduleId=1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 45, 0),
                    Duration = 15,
                    Packages = new ObservableCollection<Package>(),
                    Status = 0,
                    
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

        #region PackageGroup

        private ObservableCollection<PackageGroup> _packageGroupsRow1
        {
            get
            {
                return new ObservableCollection<PackageGroup>
                {
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Кардиология",
                        ShortName = "КРД",
                        Row = 0,
                        Color = Colors.Bisque.ToString()
                    },
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Абдоминальные",
                        ShortName = "АБД",
                        Row = 0,
                        Color = Colors.LightSkyBlue.ToString()
                    }
                };
            }
        }

        private ObservableCollection<PackageGroup> _packageGroupsRow2
        {
            get
            {
                return new ObservableCollection<PackageGroup>
                {
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Моче-половая система",
                        ShortName = "МПС",
                        Row = 1,
                        Color = Colors.LightGreen.ToString()
                    },
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Прочие",
                        ShortName = "ПРЧ",
                        Row = 1,
                        Color = Colors.HotPink.ToString()
                    },
                };
            }
        }

        public ObservableCollection<ObservableCollection<PackageGroup>> РackageGroupsRows
        {
            get
            {
                return new ObservableCollection<ObservableCollection<PackageGroup>>
                {
                    _packageGroupsRow1,
                    _packageGroupsRow2
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
                        Schedule=_scheduleFake1,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule=_schedule1,
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
                        Schedule=_scheduleFake2,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule=_schedule2,
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
                        Schedule=_scheduleFake3,
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
                        Schedule=_scheduleFake4,
                        Receptions = new ObservableCollection<Reception>()
                    },
                    new ScheduleReception
                    {
                        Schedule=_schedule3,
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
                        Schedule=_scheduleFake5,
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
                    Gender = 1,
                    City = _city1,
                    Address = "Брауншвейгская, 23, кв 2",
                    PhoneNumber = "525356",
                    MobileCode = "7705",
                    MobileNumber = "5048521",
                    Email = "patient1@mail.ru",
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
                    Gender = 1,
                    City = _city2,
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "7777",
                    MobileNumber = "4568721",
                    Email = "patient2@mail.ru",
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
                    Gender = 2,
                    City = _city3,
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "",
                    MobileNumber = "",
                    Email = "patient3@mail.ru",
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
