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
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 8, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                };
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
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
                            
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 14),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant2,
                            Cab2Variant3,
                            
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 15),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant1,
                            Cab2Variant2,
                            
                        }
                    },new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 16),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant2,
                            
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 17),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant1,
                            
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 18),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant3,
                            Cab2Variant3,
                            
                        }
                    },
                    new ScheduleDay
                    {
                        Date = new DateTime(2015, 7, 19),
                        ScheduleCabinets=new ObservableCollection<ScheduleCabinet>
                        {
                            Cab1Variant2,
                            Cab2Variant2,
                            
                        }
                    }
                };
            }
        }

        #region Static
        private static Doctor WhiteDoctor
        {
            get
            {
                return new Doctor
                {
                    Id = 0,
                    Name = "",
                    ShortName = "",
                    Color = Colors.White.ToString()
                };
            }
        }
        private static Doctor BlackDoctor
        {
            get
            {
                return new Doctor
                {
                    Id = 0,
                    Name = "",
                    ShortName = "",
                    Color = Colors.Black.ToString()
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

        public string[] CabinetHours
        {
            get
            {
                return new string[]
                {
                    "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21"
                };
            }
        }

        private ScheduleCabinet Cab1Variant1 = new ScheduleCabinet
        {
            CabinetId = "Каб.1",
            Schedules = new ObservableCollection<Schedule>
            {
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 12, 45, 0),
                    End = new DateTime(2015, 7, 11, 13, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 18, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = LGV,
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
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 7, 30, 0),
                    End = new DateTime(2015, 7, 11, 11, 55, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 11, 55, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 18, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 5, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = LGV,
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
                    
                    
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
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
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 12, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 12, 45, 0),
                    End = new DateTime(2015, 7, 11, 13, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 13, 0, 0),
                    End = new DateTime(2015, 7, 11, 18, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 18, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 45, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 2,
                    CurrentDoctor = LGV,
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
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 7, 0, 0),
                    End = new DateTime(2015, 7, 11, 8, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = Bacula,
                    Start = new DateTime(2015, 7, 11, 8, 30, 0),
                    End = new DateTime(2015, 7, 11, 11, 55, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    
                    
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 11, 55, 0),
                    End = new DateTime(2015, 7, 11, 15, 0, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = VVA,
                    Start = new DateTime(2015, 7, 11, 15, 0, 0),
                    End = new DateTime(2015, 7, 11, 16, 30, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor=WhiteDoctor,
                    Start = new DateTime(2015, 7, 11, 16, 30, 0),
                    End = new DateTime(2015, 7, 11, 18, 5, 0)
                },
                new Schedule
                {
                    Id = 0,
                    CabinetId = 1,
                    CurrentDoctor = LGV,
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
                    CurrentDoctor=WhiteDoctor,
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
                        CurrentDoctor = Bacula,
                        Start = new DateTime(2015,7,11,8,0,0),
                        End = new DateTime(2015,7,11,12,45,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        CurrentDoctor=WhiteDoctor,
                        Start = new DateTime(2015,7,11,12,45,0),
                        End = new DateTime(2015,7,11,13,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        CurrentDoctor = VVA,
                        Start = new DateTime(2015,7,11,13,0,0),
                        End = new DateTime(2015,7,11,18,30,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        
                        
                        CurrentDoctor=WhiteDoctor,
                        Start = new DateTime(2015,7,11,18,30,0),
                        End = new DateTime(2015,7,11,18,45,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 1,
                        CurrentDoctor = LGV,
                        Start = new DateTime(2015,7,11,18,45,0),
                        End = new DateTime(2015,7,11,20,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        CurrentDoctor = VVA,
                        Start = new DateTime(2015,7,11,7,30,0),
                        End = new DateTime(2015,7,11,10,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        CurrentDoctor=WhiteDoctor,
                        Start = new DateTime(2015,7,11,10,0,0),
                        End = new DateTime(2015,7,11,10,15,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        CurrentDoctor = LGV,
                        Start = new DateTime(2015,7,11,10,15,0),
                        End = new DateTime(2015,7,11,14,0,0)
                    },
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        CurrentDoctor=WhiteDoctor,
                        Start = new DateTime(2015,7,11,14,0,0),
                        End = new DateTime(2015,7,11,18,0,0)
                    },
                    
                    new Schedule
                    {
                        Id = 0,
                        CabinetId = 2,
                        CurrentDoctor = Bacula,
                        Start = new DateTime(2015,7,11,18,0,0),
                        End = new DateTime(2015,7,11,19,45,0)
                    },
                };
            }
        }
    }
}
