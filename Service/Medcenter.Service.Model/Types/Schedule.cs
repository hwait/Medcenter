using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Schedule
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CabinetId { get; set; }
        [DataMember]
        public int DoctorId { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }

        private Doctor _currentDoctor;
        [DataMember]
        public Doctor CurrentDoctor
        {
            get
            {
                return _currentDoctor;
            }
            set
            {
                _currentDoctor=value;
                DoctorId = _currentDoctor.Id;
            }
        }
        public bool ReplaceEverywhere { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string StartHour
        {
            get
            {
                return Start.Hour.ToString("00");
                //return Start.Hour.ToString("00");
            }
            set
            {
                var val = 0;
                int.TryParse(value, out val);
                Start=new DateTime(Start.Year, Start.Month, Start.Day,val,Start.Minute,0);
            }
        }
        public string StartMinute
        {
            get { return Start.Minute.ToString("00"); }
            set
            {
                var val = 0;
                int.TryParse(value, out val);
                Start = new DateTime(Start.Year, Start.Month, Start.Day,Start.Hour, val, 0);
            }
        }
        public string EndHour
        {
            get { return End.Hour.ToString("00"); }
            set
            {
                var val = 0;
                int.TryParse(value, out val);
                End = new DateTime(End.Year, End.Month, End.Day, val, End.Minute, 0);
            }
        }
        public string EndMinute
        {
            get { return End.Minute.ToString("00"); }
            set
            {
                var val = 0;
                int.TryParse(value, out val);
                End = new DateTime(End.Year, End.Month, End.Day, End.Hour, val, 0);
            }
        }
       
        public int Duration
        {
            get { return (int) End.Subtract(Start).TotalMinutes; }
        }

        public Schedule(DateTime start, DateTime end, int cabinet)
        {
            CabinetId = cabinet;
            Start = start;
            End = end;
            Id = 0;
            CurrentDoctor=new Doctor();
        }

        public Schedule()
        {
            
        }

        public void ResetFlags()
        {
            ReplaceEverywhere =false;
            Monday =false;
            Tuesday =false;
            Wednesday =false;
            Thursday =false;
            Friday =false;
            Saturday =false;
        }

        public List<ResultMessage> Validate()
        {
            //return new List<ResultMessage>();
            List<ResultMessage> em = new List<ResultMessage>();
            if (CurrentDoctor.Id==0) em.Add(new ResultMessage(2, "Доктор:", OperationErrors.VariantNotChosen));
            if (Start>=End) em.Add(new ResultMessage(2, "Время приёма:", OperationErrors.StartIsLaterThenEnd));
            return em;
        }
    }
}
