using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Misc;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.SurveysManagerModule.SampleData
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
                        "Survey"
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

        #endregion

        #region Inspections

        public ObservableCollection<Inspection> Inspections
        {
            get
            {
                return new ObservableCollection<Inspection>
                {
                    new Inspection
                    {
                        Id = 0,
                        Name = "Ультразвуковое исследование сердца",
                        ShortName = "УЗИ сердца",
                        Cost = 1000
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "К.Кардиолога",
                        Cost = 1200
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "Допплерография почек",
                        ShortName = "Д почек",
                        Cost = 3500
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "УЗИ почек",
                        Cost = 2100
                    },
                };
            }
        }

        public Inspection CurrentInspection
        {
            get
            {
                return new Inspection
                {
                    Id = 0,
                    Name = "Ультразвуковое исследование сердца",
                    ShortName = "УЗИ сердца",
                    Cost = 1000
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
                    ParaphraseId = 3,
                    PositionId = 3,
                    Status = 1,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 3,
                    DecorationType = (int)DecorationTypes.InText,
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
                    ParaphraseId = 4,
                    PositionId = 4,
                    Status = 2,
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
                    ParaphraseId = 5,
                    PositionId = 5,
                    Status = 3,
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
                    ParaphraseId = 6,
                    PositionId = 6,
                    Type = (int)PhraseTypes.String,
                    ShowOrder = 6,
                    Status = 4,
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
                    ParaphraseId = 17,
                    PositionId = 17,
                    Type = (int)PhraseTypes.Formula,
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


        #endregion
        public Phrase CurrentPhrase
        {
            get { return _phrase20; }
        }
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
                    CurrentDoctor = CurrentDoctor,
                    Phrases = new ObservableCollection<Phrase>
                    {
                        _phrase1,_phrase2,_phrase3,_phrase4,_phrase5,_phrase6,_phrase7,_phrase8,
                        _phrase9,_phrase10,_phrase11,_phrase12,_phrase13,_phrase14,_phrase15,_phrase16,
                        _phrase17,_phrase18,_phrase19,_phrase20,_phrase31,_phrase32,_phrase33,_phrase34,
                        _phrase35,_phrase36,_phrase37,_phrase38,_phrase39,_phrase40,_phrase41
                    },
                    Paraphrases = new ObservableCollection<Paraphrase> { _paraphrase1, _paraphrase2, _paraphrase3 }
                };
            }
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
                    Norm = "Стенка АО",
                    PositionId = 1,
                    ShowOrder = 1,
                    Status = 1,
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
                    Norm = "Стенка АО",
                    V1 = 3,
                    V2 = 5,
                    PositionId = 1,
                    ShowOrder = 1,
                    Status = 2,
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
                    Norm = "Створки АОК",
                    V1 = 7,
                    V2 = 15,
                    V3 = 8,
                    PositionId = 1,
                    ShowOrder = 1,
                    Status = 3,
                    FormulesAffected = new List<int>(),
                };
            }
        }

        #endregion
        public List<Paraphrase> Paraphrases
        {
            get
            {
                return new List<Paraphrase> { _paraphrase1, _paraphrase2, _paraphrase3 };
            }
        }
        public Survey CurrentSurvey
        {
            get { return _survey1; }
        }
    }
}
