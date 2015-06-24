using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.FinancesManagerModule.SampleData
{
    public class DesignData
    {
        public Discount CurrentDiscount
        {
            get
            {
                return new Discount
                {
                    Name = "Скидка ветеранам",
                    Code = "GA12",
                    IsGlobal = true,
                    Gender = 1,
                    AgeMin = 80,
                    AgeMax = 90,
                    BoughtMin = 100000,
                    BoughtMax = 1000000,
                    MonthStart = 5,
                    MonthEnd = 6,
                    DayStart = 25,
                    DayEnd = 30,
                    WeekDays = "1000001",
                    Value = 37
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

        public ObservableCollection<Discount> Discounts
        {
            get
            {
                return new ObservableCollection<Discount>
                {
                    new Discount
                    {
                        Name = "Скидка ветеранам",
                        Code = "GA12",
                        IsGlobal = true,
                        Gender = 2,
                        AgeMin = 80,
                        AgeMax = 90,
                        BoughtMin = 100000,
                        BoughtMax = 1000000,
                        MonthStart = 5,
                        MonthEnd = 6,
                        DayStart = 25,
                        DayEnd = 30,
                        WeekDays = "1000001",
                        Value=100
                    },
                    new Discount
                    {
                        Name = "Скидка под новый год",
                        Code = "GA12",
                        IsGlobal = true,
                        Gender = 0,
                        AgeMin = 80,
                        AgeMax = 90,
                        BoughtMin = 100000,
                        BoughtMax = 1000000,
                        MonthStart = 5,
                        MonthEnd = 6,
                        DayStart = 25,
                        DayEnd = 30,
                        WeekDays = "1000001",
                        Value=12
                    },
                    new Discount
                    {
                        Name = "Весенняя распродажа",
                        Code = "GA12",
                        IsGlobal = true,
                        Gender = 0,
                        AgeMin = 80,
                        AgeMax = 90,
                        BoughtMin = 100000,
                        BoughtMax = 1000000,
                        MonthStart = 5,
                        MonthEnd = 6,
                        DayStart = 25,
                        DayEnd = 30,
                        WeekDays = "1000001",
                        Value=30000
                    },
                    new Discount
                    {
                        Name = "Производсвенная скидка",
                        Code = "GA12",
                        IsGlobal = true,
                        Gender = 0,
                        AgeMin = 80,
                        AgeMax = 90,
                        BoughtMin = 100000,
                        BoughtMax = 1000000,
                        MonthStart = 5,
                        MonthEnd = 6,
                        DayStart = 25,
                        DayEnd = 30,
                        WeekDays = "1000001",
                        Value=10000
                    }
                };
            }
        }
    }
}
