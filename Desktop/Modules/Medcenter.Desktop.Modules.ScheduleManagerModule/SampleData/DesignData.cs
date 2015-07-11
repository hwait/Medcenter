using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Medcenter.Desktop.Modules.ScheduleManagerModule.Model;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.ScheduleManagerModule.SampleData
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
                        "Schedule"
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

        public Schedule CurrentSchedule
        {
            get
            {
                return new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Бацула Л.П.",
                    DoctorId = 3,
                    DoctorColor = Colors.Bisque.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
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
        public ObservableCollection<ScheduleDay> CurrentWeek
        {
            get
            {
                return new ObservableCollection<ScheduleDay>
                {
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 13),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant1,
                            Cab2Variant1,
                            CabinetHours
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 14),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant2,
                            Cab2Variant3,
                            CabinetHours
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 15),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant1,
                            Cab2Variant2,
                            CabinetHours
                        }
                    },new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 16),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant2,
                            CabinetHours
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 17),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant1,
                            CabinetHours
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 18),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant3,
                            CabinetHours
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 19),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant2,
                            Cab2Variant2,
                            CabinetHours
                        }
                    }
                };
            }
        }

        #region CabVariants
        private ScheduleCabinet CabinetHours = new ScheduleCabinet
        {
            CabinetId = "",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 7, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "7",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 7, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 8, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "8",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 8, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 9, 0, 0),
                    End = new DateTime(2015, 7, 11, 9, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "9",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 9, 0, 0),
                    End = new DateTime(2015, 7, 11, 9, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 10, 0, 0),
                    End = new DateTime(2015, 7, 11, 10, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "10",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 10, 0, 0),
                    End = new DateTime(2015, 7, 11, 10, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 11, 0, 0),
                    End = new DateTime(2015, 7, 11, 11, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "11",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 11, 0, 0),
                    End = new DateTime(2015, 7, 11, 11, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 12, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "12",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 12, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 13, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "13",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 13, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 14, 0, 0),
                    End = new DateTime(2015, 7, 11, 14, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "14",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 14, 0, 0),
                    End = new DateTime(2015, 7, 11, 14, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 15, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "15",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 15, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 16, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "16",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 16, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 17, 0, 0),
                    End = new DateTime(2015, 7, 11, 17, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "17",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 17, 0, 0),
                    End = new DateTime(2015, 7, 11, 17, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "18",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 19, 0, 0),
                    End = new DateTime(2015, 7, 11, 19, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "19",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 19, 0, 0),
                    End = new DateTime(2015, 7, 11, 19, 59, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.Black.ToString(),
                    Start = new DateTime(2015, 7, 11, 20, 0, 0),
                    End = new DateTime(2015, 7, 11, 20, 1, 0)
                },
                new Schedule
                {
                    Id = 0,
                    DoctorName = "20",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 20, 0, 0),
                    End = new DateTime(2015, 7, 11, 20, 59, 0)
                }
            }
        };
        private ScheduleCabinet Cab1Variant1 = new ScheduleCabinet
        {
            CabinetId = "Каб.1",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Бацула Л.П.",
                    DoctorId = 3,
                    DoctorColor = Colors.Bisque.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 12, 45, 0),
                    End = new DateTime(2015, 7, 11, 13, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Вдовиченко В.А.",
                    DoctorId = 1,
                    DoctorColor = Colors.Turquoise.ToString(),
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Лосева Г.В.",
                    DoctorId = 2,
                    DoctorColor = Colors.LightPink.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 45, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        private ScheduleCabinet Cab1Variant2 = new ScheduleCabinet
        {
            CabinetId = "Каб.1",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Бацула Л.П.",
                    DoctorId = 3,
                    DoctorColor = Colors.Bisque.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 30, 0),
                    End = new DateTime(2015, 7, 11, 11, 55, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 11, 55, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Вдовиченко В.А.",
                    DoctorId = 1,
                    DoctorColor = Colors.Turquoise.ToString(),
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 5, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Лосева Г.В.",
                    DoctorId = 2,
                    DoctorColor = Colors.LightPink.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 5, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        private ScheduleCabinet Cab1Variant3 = new ScheduleCabinet
        {
            CabinetId = "Каб.1",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Вдовиченко В.А.",
                    DoctorId = 1,
                    DoctorColor = Colors.Turquoise.ToString(),
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 16, 30, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        private ScheduleCabinet Cab2Variant1 = new ScheduleCabinet
        {
            CabinetId = "Каб.2",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "Бацула Л.П.",
                    DoctorId = 3,
                    DoctorColor = Colors.Bisque.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 12, 45, 0),
                    End = new DateTime(2015, 7, 11, 13, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "Вдовиченко В.А.",
                    DoctorId = 1,
                    DoctorColor = Colors.Turquoise.ToString(),
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "Лосева Г.В.",
                    DoctorId = 2,
                    DoctorColor = Colors.LightPink.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 45, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        private ScheduleCabinet Cab2Variant2 = new ScheduleCabinet
        {
            CabinetId = "Каб.2",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 8, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Бацула Л.П.",
                    DoctorId = 3,
                    DoctorColor = Colors.Bisque.ToString(),
                    Start = new DateTime(2015, 7, 11, 8, 30, 0),
                    End = new DateTime(2015, 7, 11, 11, 55, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 11, 55, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Вдовиченко В.А.",
                    DoctorId = 1,
                    DoctorColor = Colors.Turquoise.ToString(),
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 16, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 5, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    DoctorName = "Лосева Г.В.",
                    DoctorId = 2,
                    DoctorColor = Colors.LightPink.ToString(),
                    Start = new DateTime(2015, 7, 11, 18, 5, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        private ScheduleCabinet Cab2Variant3 = new ScheduleCabinet
        {
            CabinetId = "Каб.2",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    DoctorName = "",
                    DoctorId = 0,
                    DoctorColor = Colors.White.ToString(),
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 20, 0, 0)
                }
            }
        };

        #endregion

        public ObservableCollection<Schedule> Schedules
        {
            get
            {
                return new ObservableCollection<Schedule>
                {
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        DoctorName = "Бацула Л.П.",
                        DoctorId = 3,
                        DoctorColor = Colors.Bisque.ToString(),
                        Start = new DateTime(2015,7,11,8,0,0),
                        End = new DateTime(2015,7,11,12,45,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        DoctorName = "",
                        DoctorId = 0,
                        DoctorColor = Colors.White.ToString(),
                        Start = new DateTime(2015,7,11,12,45,0),
                        End = new DateTime(2015,7,11,13,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        DoctorName = "Вдовиченко В.А.",
                        DoctorId = 1,
                        DoctorColor = Colors.Turquoise.ToString(),
                        Start = new DateTime(2015,7,11,13,0,0),
                        End = new DateTime(2015,7,11,18,30,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        DoctorName = "",
                        DoctorId = 0,
                        DoctorColor = Colors.White.ToString(),
                        Start = new DateTime(2015,7,11,18,30,0),
                        End = new DateTime(2015,7,11,18,45,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        DoctorName = "Лосева Г.В.",
                        DoctorId = 2,
                        DoctorColor = Colors.LightPink.ToString(),
                        Start = new DateTime(2015,7,11,18,45,0),
                        End = new DateTime(2015,7,11,20,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        DoctorName = "Вдовиченко В.А.",
                        DoctorId = 1,
                        DoctorColor = Colors.PaleGreen.ToString(),
                        Start = new DateTime(2015,7,11,7,30,0),
                        End = new DateTime(2015,7,11,10,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        DoctorName = "",
                        DoctorId = 0,
                        DoctorColor = Colors.White.ToString(),
                        Start = new DateTime(2015,7,11,10,0,0),
                        End = new DateTime(2015,7,11,10,15,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        DoctorName = "Лосева Г.В.",
                        DoctorId = 2,
                        DoctorColor = Colors.LightSkyBlue.ToString(),
                        Start = new DateTime(2015,7,11,10,15,0),
                        End = new DateTime(2015,7,11,14,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        DoctorName = "",
                        DoctorId = 0,
                        DoctorColor = Colors.White.ToString(),
                        Start = new DateTime(2015,7,11,14,0,0),
                        End = new DateTime(2015,7,11,18,0,0)
                    },
                    
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        DoctorName = "Бацула Л.П.",
                        DoctorId = 3,
                        DoctorColor = Colors.PaleVioletRed.ToString(),
                        Start = new DateTime(2015,7,11,18,0,0),
                        End = new DateTime(2015,7,11,19,45,0)
                    },
                };
            }
        }
    }
}
