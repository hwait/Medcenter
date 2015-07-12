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
        public string DoctorName { get; set; }
        [DataMember]
        public int DoctorId { get; set; }
        [DataMember]
        public string DoctorColor { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }
        public Doctor CurrentDoctor { get; set; }
        public string StartHour
        {
            get { return Start.Hour.ToString("00"); }
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
        public List<ResultMessage> Validate()
        {
            return new List<ResultMessage>();
            //List<ResultMessage> em = new List<ResultMessage>();
            //if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            //if (string.IsNullOrEmpty(ShortName)) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.EmptyString));
            //return em;
        }
    }
}
