using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Reception
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public int PatientId { get; set; }
        [DataMember]
        public Schedule Schedule { get; set; }
        [DataMember]
        public Discount Discount { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public ObservableCollection<Package> Packages { get; set; }
        [DataMember]
        public byte Status { get; set; }
        [DataMember]
        public Payment Payment { get; set; }
        public string Text {
            get { return String.Join(", ", Packages.Select(i => i.ShortName).ToArray()); }
            }
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
                Start = new DateTime(Start.Year, Start.Month, Start.Day, val, Start.Minute, 0);
            }
        }
        public string StartMinute
        {
            get { return Start.Minute.ToString("00"); }
            set
            {
                var val = 0;
                int.TryParse(value, out val);
                Start = new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, val, 0);
            }
        }
        public Reception()
        {
            Packages = new ObservableCollection<Package>();
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            return em;
        }
    }
}
