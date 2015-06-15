﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.InspectionsManagerModule.SampleData
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

        public InspectionGroup CurrentInspectionGroup
        {
            get
            {
                return new InspectionGroup
                {
                    Id = 0,
                    Name = "Кардиология",
                    ShortName = "КРД",
                    Row = 0,
                    Color = Colors.Bisque.ToString()
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
                    ShortName = "ЭХО",
                    Duration = 10,
                    InspectionGroupId = 0,
                    Cost = 1000
                };
            }
        }

        public ObservableCollection<InspectionGroup> InspectionGroups
        {
            get
            {
                return new ObservableCollection<InspectionGroup>
                {
                    new InspectionGroup
                    {
                        Id = 0,
                        Name = "Кардиология",
                        ShortName = "КРД",
                        Row = 0,
                        Color = Colors.Bisque.ToString()
                    },
                    new InspectionGroup
                    {
                        Id = 0,
                        Name = "Абдоминальные",
                        ShortName = "АБД",
                        Row = 0,
                        Color = Colors.LightSkyBlue.ToString()
                    },
                    new InspectionGroup
                    {
                        Id = 0,
                        Name = "Моче-половая система",
                        ShortName = "МПС",
                        Row = 0,
                        Color = Colors.LightGreen.ToString()
                    },
                    new InspectionGroup
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
                        ShortName = "ЭХО",
                        Duration = 10,
                        InspectionGroupId = 0,
                        Cost = 1000
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "Консультация кардиолога",
                        ShortName = "Кардиолог",
                        Duration = 5,
                        InspectionGroupId = 0,
                        Cost = 1200
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "УЗИ почек с допплерографией",
                        ShortName = "УЗДГ почек",
                        Duration = 5,
                        InspectionGroupId = 1,
                        Cost = 3500
                    },
                    new Inspection
                    {
                        Id = 0,
                        Name = "УЗИ почек",
                        ShortName = "Почки",
                        Duration = 10,
                        InspectionGroupId = 1,
                        Cost = 2100
                    },
                };
            }
        }
    }
}
