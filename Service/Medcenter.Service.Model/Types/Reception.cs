using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Misc;
using Medcenter.Service.Model.Operations;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Reception : INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int RefererId { get; set; }
        [DataMember]
        public int PatientId { get; set; }

        [DataMember]
        public Patient Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Patient"));
            }
        }

        [DataMember]
        public int ScheduleId { get; set; }

        [DataMember]
        public int Cost
        {
            get
            {
                _cost = 0;
                foreach (var package in Packages)
                {
                    _cost += package.Cost;
                }
                return _cost;
            }
            set
            {
                _cost = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Paid"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ToPay"));
                }
            }
        }
        

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
            }
        }

        [DataMember]
        public ObservableCollection<Package> Packages
        {
            get { return _packages; }
            set
            {
                _packages = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Paid"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ToPay"));
                }
            }
        }

        [DataMember]
        public byte Status { get; set; }

        [DataMember]
        public ObservableCollection<Payment> Payments { get; set; }
        public int MaxDuration { get; set; }
        public int Paid
        {
            get
            {
                var c = 0;
                foreach (var payment in Payments)
                {
                    c += payment.Cost;
                }
                return c;
            }
        }
        public int ToPay { get { return Cost-Paid; } }
        private int _cost;

        public Payment CurrentPayment
        {
            get { return _currentPayment; }
            set { _currentPayment = value; }
        }

        private Patient _patient;
        private int _duration;
        private ObservableCollection<Package> _packages;
        private Payment _currentPayment;

        public string StatusText
        {
            get { return Statuses.GetStatus(Status); }
        }
        
        public string Text 
        {
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
        
        public void CalcDuration()
        {
            int dur = 0;
            foreach (var package in Packages)
            {
                dur += package.Duration;
            }
            Duration = MaxDuration < dur ? MaxDuration : dur;
        }  
        public Reception()
        {
            Packages = new ObservableCollection<Package>();
            Payments=new ObservableCollection<Payment>();
        }

        public Reception(int scheduleId,DateTime start, int duration)
        {
            ScheduleId = scheduleId;
            Start = start;
            Duration = duration;
            Packages = new ObservableCollection<Package>();
            Payments = new ObservableCollection<Payment>();
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (Duration<5) em.Add(new ResultMessage(2, "Длительность:", OperationErrors.TooSmall));
            if (Packages.Count == 0) em.Add(new ResultMessage(2, "Исследования:", OperationErrors.VariantNotChosen));
            return em;
        }

        public void ActuateProperties()
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Discount"));
        }
        public event PropertyChangedEventHandler PropertyChanged;


    }
}
