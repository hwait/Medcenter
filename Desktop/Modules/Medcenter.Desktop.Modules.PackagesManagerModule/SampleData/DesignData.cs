using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.PackagesManagerModule.SampleData
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
        public Discount CurrentDiscount
        {
            get
            {
                return new Discount
                {
                    Name = "Скидка ветеранам",
                    ShortName = "Ветеранам 37%",
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
                    ValueText = "37",
                    Value = 37
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
                        ShortName = "Ветеранам 37%",
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
                        ShortName = "НГ 12%",
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
                        ShortName = "8 марта 5%",
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
                        Name = "Производственная скидка",
                        ShortName = "Юр.лиц.10%",
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
        public ObservableCollection<Discount> DiscountsInPackage
        {
            get
            {
                return new ObservableCollection<Discount>
                {
                    new Discount
                    {
                        Name = "Весенняя распродажа",
                        ShortName = "8 марта 5%",
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
                        Name = "Производственная скидка",
                        ShortName = "Юр.лиц.10%",
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
        public ObservableCollection<Inspection> InspectionsInPackage
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

        public PackageGroup CurrentPackageGroup
        {
            get
            {
                return new PackageGroup
                {
                    Id = 0,
                    Name = "Кардиология",
                    ShortName = "КРД",
                    Row = 0,
                    //Color = null//(Color) Colors.Bisque//.ToString()
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
                    PackageGroupId = 0,
                    Cost = 1000
                };
            }
        }

        public ObservableCollection<PackageGroup> PackageGroups
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
                        //Color = null;//Colors.Bisque.ToString()
                    },
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Абдоминальные",
                        ShortName = "АБД",
                        Row = 0,
                        //Color = Colors.LightSkyBlue.ToString()
                    },
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Моче-половая система",
                        ShortName = "МПС",
                        Row = 0,
                        //Color = Colors.LightGreen.ToString()
                    },
                    new PackageGroup
                    {
                        Id = 0,
                        Name = "Прочие",
                        ShortName = "ПРЧ",
                        Row = 1,
                        //Color = Colors.HotPink.ToString()
                    },
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
                        PackageGroupId = 0,
                        Cost = 1000
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "Кардиолог",
                        Duration = 5,
                        PackageGroupId = 0,
                        Cost = 1200
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек с допплерографией",
                        ShortName = "УЗДГ почек",
                        Duration = 5,
                        PackageGroupId = 1,
                        Cost = 3500
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "Почки",
                        Duration = 10,
                        PackageGroupId = 1,
                        Cost = 2100
                    },
                };
            }
        }
        public ObservableCollection<Package> PackagesInGroup
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
                        PackageGroupId = 0,
                        Cost = 1000
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "Кардиолог",
                        Duration = 5,
                        PackageGroupId = 0,
                        Cost = 1200
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек с допплерографией",
                        ShortName = "УЗДГ почек",
                        Duration = 5,
                        PackageGroupId = 1,
                        Cost = 3500
                    },
                    new Package
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "Почки",
                        Duration = 10,
                        PackageGroupId = 1,
                        Cost = 2100
                    },
                };
            }
        }
    }
}
