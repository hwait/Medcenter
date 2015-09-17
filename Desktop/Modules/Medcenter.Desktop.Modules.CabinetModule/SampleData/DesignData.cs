using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Misc;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.CabinetModule.SampleData
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

        private Patient _currentPatient
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
                    Address = "Брауншвейгская, 23, кв 2",
                    PhoneNumber = "525356",
                    MobileCode = "7705",
                    MobileNumber = "5048521",
                    Email = "patient1@mail.ru",
                    Receptions = new List<Reception>()
                };
            }
        }
        public Reception CurrentReception
        {
            get { return _reception22; }
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
                    Address = "Брауншвейгская, 23, кв 2",
                    PhoneNumber = "525356",
                    MobileCode = "7705",
                    MobileNumber = "5048521",
                    Email = "patient1@mail.ru",
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
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "7777",
                    MobileNumber = "4568721",
                    Email = "patient2@mail.ru",
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
                    Address = "",
                    PhoneNumber = "",
                    MobileCode = "",
                    MobileNumber = "",
                    Email = "patient3@mail.ru",
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


        #region Receptions 2
        
        private Payment _payment1
        {
            get
            {
                return new Payment
                {
                    Id = 0,
                    Cost = 5600,
                    OldCost = 5600,
                    ReceptionId = 7,
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0)
                };
            }
        }
        private Payment _payment2
        {
            get
            {
                return new Payment
                {
                    Id = 2,
                    Cost = 1400,
                    OldCost = 1330,
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
                    CurrentPayment = _payment2,
                    PatientId = 2,
                    Patient = _currentPatient,
                    ScheduleId = 1,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 10, 0),
                    Duration = 25,
                    Packages = _packages1,
                    Status = 2
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
                    PatientId = 3,
                    Patient = _patient1,
                    ScheduleId = 2,
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
                    PatientId = 1,
                    Patient = _patient2,
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
                    Patient = _patient3,
                    ScheduleId = 2,
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 50, 0),
                    Duration = 20,
                    Packages = _packages4,
                    Status = 1,
                };
            }
        }
        
        #endregion
        
        public List<Reception> Receptions
        {
            get
            {
                return new List<Reception>
                {
                    _reception22,_reception23,_reception24,_reception25
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

        public ObservableCollection<Doctor> Doctors
        {
            get
            {
                return new ObservableCollection<Doctor>
                {
                    Bacula,VVA,LGV
                };
            }
        }

        public Doctor CurrentDoctor
        {
            get
            {
                return Bacula;
            }
        }

        #endregion

        #region Schedules

        public Schedule CurrentSchedule
        {
            get
            {
                return new Schedule
                {
                    Id = 1,
                    CabinetId = 1,
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                };
            }
        }

        public ObservableCollection<Schedule> Schedules
        {
            get
            {
                return new ObservableCollection<Schedule>
                {
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 1,
                        CurrentDoctor = Bacula,
                        ShowName = Bacula.ShortName,
                        Start = new DateTime(2015, 7, 11, 8, 0, 0),
                        End = new DateTime(2015, 7, 11, 12, 45, 0)
                    },
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 1,
                        CurrentDoctor = VVA,
                        ShowName = VVA.ShortName,
                        Start = new DateTime(2015, 7, 11, 13, 0, 0),
                        End = new DateTime(2015, 7, 11, 18, 30, 0)
                    },
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 1,
                        CurrentDoctor = LGV,
                        ShowName = LGV.ShortName,
                        Start = new DateTime(2015, 7, 11, 18, 45, 0),
                        End = new DateTime(2015, 7, 11, 20, 0, 0)
                    },
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 2,
                        CurrentDoctor = VVA,
                        ShowName = VVA.ShortName,
                        Start = new DateTime(2015, 7, 11, 7, 30, 0),
                        End = new DateTime(2015, 7, 11, 10, 0, 0)
                    },
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 2,
                        CurrentDoctor = LGV,
                        ShowName = LGV.ShortName,
                        Start = new DateTime(2015, 7, 11, 10, 15, 0),
                        End = new DateTime(2015, 7, 11, 14, 0, 0)
                    },
                    new Schedule
                    {
                        Id = 1,
                        CabinetId = 2,
                        CurrentDoctor = Bacula,
                        ShowName = Bacula.ShortName,
                        Start = new DateTime(2015, 7, 11, 18, 0, 0),
                        End = new DateTime(2015, 7, 11, 19, 45, 0)
                    },
                };
            }
        }

        #endregion


        #region Phrases

        private Phrase _phrase1
        {
            get
            {
                return new Phrase
                {
                    Id = 1,
                    PositionName = "ПАРЕНХИМА МОЗГА",
                    PositionId = 1,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 1,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        private Phrase _phrase2
        {
            get
            {
                return new Phrase
                {
                    Id = 2,
                    PositionName = "Эхоструктура",
                    Text = " однородная. ",
                    ParaphraseId = 2,
                    PositionId = 2,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 2,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase3
        {
            get
            {
                return new Phrase
                {
                    Id = 3,
                    PositionName = "Расположение",
                    Text = "Прилегает к кости. ",
                    ParaphraseId =3,
                    PositionId = 3,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 3,
                    DecorationType = (int) DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase4
        {
            get
            {
                return new Phrase
                {
                    Id = 4,
                    PositionName = "Борозды",
                    Text = " Рисунок  извилин и борозд четкий. Поясная борозда глубокая. ",
                    ParaphraseId = 4,
                    PositionId = 4,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 4,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase5
        {
            get
            {
                return new Phrase
                {
                    Id = 5,
                    PositionName = "Субкортикальные зоны:",
                    Text = " Справа эхогенность не изменена. Слева эхогенность не изменена. ",
                    ParaphraseId = 5,
                    PositionId = 5,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 5,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase6
        {
            get
            {
                return new Phrase
                {
                    Id = 6,
                    PositionName = "Перивентрикулярная область:",
                    Text = " не изменена. ",
                    ParaphraseId = 6,
                    PositionId = 6,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 6,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase7
        {
            get
            {
                return new Phrase
                {
                    Id = 7,
                    PositionName = "ЖЕЛУДОЧКОВАЯ СИСТЕМА",
                    ParaphraseId = 7,
                    PositionId = 7,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 7,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase8
        {
            get
            {
                return new Phrase
                {
                    Id = 8,
                    PositionName = "Боковые желудочки:",
                    Text = " полости свободны, анэхогенны. ",
                    ParaphraseId = 8,
                    PositionId = 8,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 8,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase9
        {
            get
            {
                return new Phrase
                {
                    Id = 9,
                    PositionName = "III желудочек",
                    Text = " не расширен. ",
                    ParaphraseId = 9,
                    PositionId = 9,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 9,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase10
        {
            get
            {
                return new Phrase
                {
                    Id = 10,
                    PositionName = "IV желудочек",
                    Text = " в S0  не  расширен, треугольной формы. ",
                    ParaphraseId = 10,
                    PositionId = 10,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 10,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase11
        {
            get
            {
                return new Phrase
                {
                    Id = 11,
                    PositionName = "СОСУДИСТЫЕ СПЛЕТЕНИЯ",
                    PositionId = 11,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 11,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase12
        {
            get
            {
                return new Phrase
                {
                    Id = 12,
                    PositionName = "Форма",
                    Text = " Симметричные. ",
                    ParaphraseId = 12,
                    PositionId = 12,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 12,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase13
        {
            get
            {
                return new Phrase
                {
                    Id = 13,
                    PositionName = "Контуры справа",
                    Text = " четкие, не ровные. ",
                    ParaphraseId = 13,
                    PositionId = 13,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 13,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase14
        {
            get
            {
                return new Phrase
                {
                    Id = 14,
                    PositionName = "Контуры слева",
                    Text = " в структуре определяется объёмное образование размерами {0}x{1}x{2} мм. ",
                    V1 = 7,
                    V2 = 15,
                    V3 = 8,
                    ParaphraseId = 14,
                    PositionId = 14,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 14,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase15
        {
            get
            {
                return new Phrase
                {
                    Id = 15,
                    PositionName = "Размеры:",
                    Text = " Ширина F6 справа {0} мм, слева {1} мм. ",
                    V1 = 3,
                    V2 = 4,
                    ParaphraseId = 15,
                    PositionId = 15,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 15,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase16
        {
            get
            {
                return new Phrase
                {
                    Id = 16,
                    PositionName = "СРЕДИННЫЕ СТРУКТУРЫ МОЗГА:",
                    PositionId = 16,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 16,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase17
        {
            get
            {
                return new Phrase
                {
                    Id = 17,
                    PositionName = "Смещение",
                    Text = " отсутствует. ",
                    ParaphraseId = 17,
                    PositionId = 17,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 17,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase18
        {
            get
            {
                return new Phrase
                {
                    Id = 18,
                    PositionName = "Ножки мозга",
                    Text = " Ножки мозга четкие, ровные симметричные, гипоэхогенные. ",
                    ParaphraseId = 18,
                    PositionId = 18,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 18,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase19
        {
            get
            {
                return new Phrase
                {
                    Id = 19,
                    PositionName = "Водопровод мозга",
                    Text = " в режиме  S0 = {0}-{1} мм. ",
                    V1 = 3,
                    V2 = 4,
                    ParaphraseId = 19,
                    PositionId = 19,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 19,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase20
        {
            get
            {
                return new Phrase
                {
                    Id = 20,
                    PositionName = "Шишковидная железа",
                    Text = " визуализируется. ",
                    ParaphraseId = 20,
                    PositionId = 20,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 20,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase31
        {
            get
            {
                return new Phrase
                {
                    Id = 31,
                    PositionName = "ЦИСТЕРНЫ МОЗГА В РЕЖИМЕ ТН0",
                    PositionId = 31,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 31,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        private Phrase _phrase32
        {
            get
            {
                return new Phrase
                {
                    Id = 32,
                    PositionName = "Вывороты",
                    Text = " Визуализация выворотов рисунка базальных цистерн достаточная, деформаций нет. ",
                    ParaphraseId = 32,
                    PositionId = 32,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 32,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase33
        {
            get
            {
                return new Phrase
                {
                    Id = 33,
                    PositionName = "Пульсация",
                    Text = " Пульсация рисунка базальных цистерн достаточная. ",
                    ParaphraseId = 33,
                    PositionId = 33,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 33,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase34
        {
            get
            {
                return new Phrase
                {
                    Id = 34,
                    PositionName = "Щель и зоны",
                    Text = " Зона Сильвиевой щели, хиазмально-селлярная, прехиазмальная зона не изменена. ",
                    ParaphraseId = 34,
                    PositionId = 34,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 34,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase35
        {
            get
            {
                return new Phrase
                {
                    Id = 5,
                    PositionName = "Охватывающая цистерна:",
                    Text = " рисунок не изменен. без изменений. без  особенностей.. ",
                    ParaphraseId = 35,
                    PositionId = 35,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 35,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase36
        {
            get
            {
                return new Phrase
                {
                    Id = 36,
                    PositionName = "Межножковая цистерна:",
                    Text = " рисунок не изменен. без изменений. без  особенностей. ",
                    ParaphraseId = 36,
                    PositionId = 36,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 36,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase37
        {
            get
            {
                return new Phrase
                {
                    Id = 37,
                    PositionName = "ЗАКЛЮЧЕНИЕ:",
                    ParaphraseId = 37,
                    PositionId = 37,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 37,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase38
        {
            get
            {
                return new Phrase
                {
                    Id = 38,
                    PositionName = "",
                    Text = " Лёгкие гипорезорбции в парасаггитальной области (транзитиорные). ",
                    ParaphraseId = 38,
                    PositionId = 37,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 38,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase39
        {
            get
            {
                return new Phrase
                {
                    Id = 39,
                    PositionName = "",
                    Text = " Описание 2. ",
                    ParaphraseId = 39,
                    PositionId = 37,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 39,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase40
        {
            get
            {
                return new Phrase
                {
                    Id = 38,
                    PositionName = "",
                    Text = " Описание 3. ",
                    ParaphraseId = 38,
                    PositionId = 38,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 38,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        
        private Phrase _phrase41
        {
            get
            {
                return new Phrase
                {
                    Id = 40,
                    PositionName = "РЕКОМЕНДАЦИИ:",
                    PositionId = 40,
                    Type = (int)PhraseTypes.Header,
                    ShowOrder = 40,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase42
        {
            get
            {
                return new Phrase
                {
                    Id = 39,
                    PositionName = "",
                    Text = "",
                    ParaphraseId = 0,
                    PositionId = 39,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 39,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }
        private Phrase _phrase43
        {
            get
            {
                return new Phrase
                {
                    Id = 39,
                    PositionName = "",
                    Text = "",
                    ParaphraseId = 0,
                    PositionId = 39,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 39,
                    DecorationType = (int)DecorationTypes.InText,
                    FormulesAffected = new List<int>(),
                };
            }
        }


        #endregion
        
        public Phrase CurrentPhrase
        {
            get { return _phrase20; }
        }

        #region Paraphrases

        private Paraphrase _paraphrase1
        {
            get
            {
                return new Paraphrase
                {
                    Id = 2,
                    Text = " однородная. ",
                    PositionId = 1,
                    ShowOrder = 1,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        private Paraphrase _paraphrase2
        {
            get
            {
                return new Paraphrase
                {
                    Id = 2,
                    Text = " неоднородная, с включениями диаметром {0}-{1} мм. ",
                    V1 = 3,
                    V2 = 5,
                    PositionId = 1,
                    ShowOrder = 1,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        private Paraphrase _paraphrase3
        {
            get
            {
                return new Paraphrase
                {
                    Id = 2,
                    Text = " в структуре определяется объёмное образование размерами {0}x{1}x{2} мм. ",
                    V1 = 7,
                    V2 = 15,
                    V3 = 8,
                    PositionId = 1,
                    ShowOrder = 1,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        #endregion
        public List<Paraphrase> Paraphrases
        {
            get 
            {
                return new List<Paraphrase>{_paraphrase1, _paraphrase2, _paraphrase3 } ;
            }
        }

        #region Surveys

        private Survey _survey1
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "НСГ откр.род.",
                    Name = "ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ",
                    Header = "Header for ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ",
                    Status = 2,
                    CurrentDoctor = CurrentDoctor,
                    CurrentPatient = CurrentReception.Patient,
                    Paraphrases = new ObservableCollection<Paraphrase>
                    {
                        _paraphrase1, _paraphrase2, _paraphrase3
                    },
                    Phrases = new ObservableCollection<Phrase>
                    {
                        _phrase1,
                        _phrase2,
                        _phrase3,
                        _phrase4,
                        _phrase5,
                        _phrase6,
                        _phrase7,
                        _phrase8,
                        _phrase9,
                        _phrase10,
                        _phrase11,
                        _phrase12,
                        _phrase13,
                        _phrase14,
                        _phrase15,
                        _phrase16,
                        _phrase17,
                        _phrase18,
                        _phrase19,
                        _phrase20,
                        _phrase31,
                        _phrase32,
                        _phrase33,
                        _phrase34,
                        _phrase35,
                        _phrase36,
                        _phrase37,
                        _phrase38,
                        _phrase39,
                        _phrase40,
                        _phrase41,
                        _phrase42,
                        _phrase43
                    }
                };
            }
        }

        private Survey _survey2
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "УЗИ сердца и сосудов",
                    Name = "ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ 2",
                    Header = "Header for ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ 2",
                    Status = 1,
                    CurrentDoctor = CurrentDoctor,
                    CurrentPatient = CurrentReception.Patient,
                    Phrases = new ObservableCollection<Phrase>
                    {
                        _phrase1,
                        _phrase2,
                        _phrase3,
                        _phrase4,
                        _phrase5,
                        _phrase6,
                        _phrase7,
                        _phrase8,
                        _phrase9,
                        _phrase10,
                        _phrase11,
                        _phrase12,
                        _phrase13,
                        _phrase14,
                        _phrase15,
                        _phrase16,
                        _phrase17,
                        _phrase18,
                        _phrase19,
                        _phrase20,
                        _phrase31,
                        _phrase32,
                        _phrase33,
                        _phrase34,
                        _phrase35,
                        _phrase36,
                        _phrase37,
                        _phrase38,
                        _phrase39,
                        _phrase40,
                        _phrase41,
                        _phrase42,
                        _phrase43
                    }
                };
            }
        }

        private Survey _survey3
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "ШОП и УЗДГ сосудов шеи и головы",
                    Name = "ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ 3",
                    Header = "Header for ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ 3",
                    Status = 1,
                    CurrentDoctor = CurrentDoctor,
                    CurrentPatient = CurrentReception.Patient,
                    Phrases = new ObservableCollection<Phrase>
                    {
                        _phrase1,
                        _phrase2,
                        _phrase3,
                        _phrase4,
                        _phrase5,
                        _phrase6,
                        _phrase7,
                        _phrase8,
                        _phrase9,
                        _phrase10,
                        _phrase11,
                        _phrase12,
                        _phrase13,
                        _phrase14,
                        _phrase15,
                        _phrase16,
                        _phrase17,
                        _phrase18,
                        _phrase19,
                        _phrase20,
                        _phrase31,
                        _phrase32,
                        _phrase33,
                        _phrase34,
                        _phrase35,
                        _phrase36,
                        _phrase37,
                        _phrase38,
                        _phrase39,
                        _phrase40,
                        _phrase41,
                        _phrase42,
                        _phrase43
                    }
                };
            }
        }

        private Survey _survey4
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "НСГ откр.род.",
                    Date = new DateTime(2015, 6, 6)
                };
            }
        }

        private Survey _survey5
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "НСГ откр.род.2",
                    Date = new DateTime(2015, 6, 8)
                };
            }
        }

        private Survey _survey6
        {
            get
            {
                return new Survey
                {
                    Id = 1,
                    ShortName = "НСГ откр.род.3",
                    Date = new DateTime(2015, 7, 16)
                };
            }
        }

        #endregion

        public Survey CurrentSurvey
        {
            get { return _survey1; }
        }
        public List<Survey> Surveys
        {
            get { return new List<Survey> { _survey1, _survey2, _survey3 }; }
        }
        public List<Survey> FormerSurveys
        {
            get { return new List<Survey> { _survey4, _survey5, _survey6 }; }
        }
    }
}
