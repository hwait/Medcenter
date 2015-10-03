using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Desktop.Modules.CabinetModule.Model;
using Medcenter.Service.Model.Misc;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.CabinetModule.SampleData
{
    public class PrintData
    {
        private Patient _patient
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
        private DocumentPicture _picture
        {
            get
            {
                return new DocumentPicture
                {
                    Path = @"c:\Users\Nikk\Documents\Projects\Medcenter\Desktop\Modules\Medcenter.Desktop.Modules.CabinetModule\Pictures\NEURO1.jpg",
                    DecorationType = 0, //0 - Left, 1 - Center, 2 - Right
                };
            }
        }
        #region PrintPhrases
        
        private PrintPhrase _printPhrase1
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ПАРЕНХИМА МОЗГА",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                };
            }
        }

        private PrintPhrase _printPhrase2
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Эхоструктура",
                    Text = " однородная. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                };
            }
        }
        private PrintPhrase _printPhrase3
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Расположение",
                    Text = "Прилегает к кости. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                };
            }
        }
        private PrintPhrase _printPhrase4
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Борозды",
                    Text = " Рисунок  извилин и борозд четкий. Поясная борозда глубокая. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                };
            }
        }
        private PrintPhrase _printPhrase5
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Субкортикальные зоны:",
                    Text = " Справа эхогенность не изменена. Слева эхогенность не изменена. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                };
            }
        }
        private PrintPhrase _printPhrase6
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Перивентрикулярная область:",
                    Text = " не изменена. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                };
            }
        }
        private PrintPhrase _printPhrase7
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЖЕЛУДОЧКОВАЯ СИСТЕМА",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                };
            }
        }
        private PrintPhrase _printPhrase8
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Боковые желудочки:",
                    Text = " полости свободны, анэхогенны. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                };
            }
        }
        private PrintPhrase _printPhrase9
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "III желудочек",
                    Text = " не расширен. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                };
            }
        }
        private PrintPhrase _printPhrase10
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "IV желудочек",
                    Text = " в S0  не  расширен, треугольной формы. ",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                };
            }
        }
        private PrintPhrase _printPhrase11
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "СОСУДИСТЫЕ СПЛЕТЕНИЯ",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                };
            }
        }
        private PrintPhrase _printPhrase12
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Форма",
                    Text = " Симметричные. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase13
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Контуры справа",
                    Text = " четкие, не ровные. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase14
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Контуры слева",
                    Text = " в структуре определяется объёмное образование размерами 12x9x7 мм. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase15
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Размеры:",
                    Text = " Ширина F6 справа 6 мм, слева 5 мм. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase16
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "СРЕДИННЫЕ СТРУКТУРЫ МОЗГА:",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    
                };
            }
        }
        private PrintPhrase _printPhrase17
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Смещение",
                    Text = " отсутствует. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase18
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Ножки мозга",
                    Text = "ПМА Vs=78 см/с, ниже нормы (<80). ПМА Vd=26 см/с. ПМА RI=0,5, ниже нормы (<0,61).",
                    Type = (int)PhraseTypes.Number,
                    DecorationType = (int)DecorationTypes.StartsAndEndsWithNewParagraph,
                    
                };
            }
        }
        private PrintPhrase _printPhrase19
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Водопровод мозга",
                    Text = " в режиме  S0 = 3-4 мм. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase20
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Шишковидная железа",
                    Text = " визуализируется. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase31
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЦИСТЕРНЫ МОЗГА В РЕЖИМЕ ТН0",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    
                };
            }
        }

        private PrintPhrase _printPhrase32
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Вывороты",
                    Text = " Визуализация выворотов рисунка базальных цистерн достаточная, деформаций нет. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase33
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Пульсация",
                    Text = " Пульсация рисунка базальных цистерн достаточная. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase34
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Щель и зоны",
                    Text = " Зона Сильвиевой щели, хиазмально-селлярная, прехиазмальная зона не изменена. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase35
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Охватывающая цистерна:",
                    Text = " рисунок не изменен. без изменений. без  особенностей.. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase36
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "Межножковая цистерна:",
                    Text = " рисунок не изменен. без изменений. без  особенностей. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InTextWithPosition,
                    
                };
            }
        }
        private PrintPhrase _printPhrase37
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЗАКЛЮЧЕНИЕ:",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    
                };
            }
        }
        private PrintPhrase _printPhrase38
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЗАКЛЮЧЕНИЕ",
                    Text = "Эхоструктура и архетектоника мозга не изменена. Нарушения ликвородинамики не выявлено.",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase39
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЗАКЛЮЧЕНИЕ",
                    Text = "При визуальной оценке сосудистый рисунок не изменён. Пульсация достаточна. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase40
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "ЗАКЛЮЧЕНИЕ",
                    Text = "Лёгкие гипорезорбции в парасаггитальной области (транзитиорные). Очагов патологической плотности на момент осмотра не выявлены. ",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }

        private PrintPhrase _printPhrase41
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "РЕКОМЕНДАЦИИ:",
                    Type = (int)PhraseTypes.Header,
                    DecorationType = (int)DecorationTypes.HeaderOnly,
                    
                };
            }
        }
        private PrintPhrase _printPhrase42
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "РЕКОМЕНДАЦИИ",
                    Text = "Проведение мониторинга в 3 мес. (НСГ + сосуды головного мозга).",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                    
                };
            }
        }
        private PrintPhrase _printPhrase43
        {
            get
            {
                return new PrintPhrase
                {
                    Header = "РЕКОМЕНДАЦИИ",
                    Text = "Консультация невропатолога.",
                    Type = (int)PhraseTypes.String,
                    DecorationType = (int)DecorationTypes.InText,
                };
            }
        }


        #endregion
        public PrintDocument Doc
        {
            get
            {
                return new PrintDocument
                {
                    Id = 1,
                    Name = "ТРАНСКРАНИАЛЬНАЯ УЛЬТРАСОНОГРАФИЯ С ДОППЛЕРОМЕТРИЕЙ СОСУДОВ",
                    Header = "С 03.004.003",
                    Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Month, 10, 30, 0),
                    Patient=_patient,
                    Signature = "Наиколлегиальнейше ______________ кмн Ацина С.В.",
                    Picture = _picture,
                    PrintPhrases=new List<PrintPhrase>
                    {
                        _printPhrase1,
                        _printPhrase2,
                        _printPhrase3,
                        _printPhrase4,
                        _printPhrase5,
                        _printPhrase6,
                        _printPhrase7,
                        _printPhrase8,
                        _printPhrase9,
                        _printPhrase10,
                        _printPhrase11,
                        _printPhrase12,
                        _printPhrase13,
                        _printPhrase14,
                        _printPhrase15,
                        _printPhrase16,
                        _printPhrase17,
                        _printPhrase18,
                        _printPhrase19,
                        _printPhrase20,
                        _printPhrase31,
                        _printPhrase32,
                        _printPhrase33,
                        _printPhrase34,
                        _printPhrase35,
                        _printPhrase36,
                        _printPhrase37,
                        _printPhrase38,
                        _printPhrase39,
                        _printPhrase40,
                        _printPhrase41,
                        _printPhrase42,
                        _printPhrase43
                    }
                };
            }
        }
    }
}
