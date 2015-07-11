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
