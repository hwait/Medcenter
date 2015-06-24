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
    public class Discount
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }
        [DataMember]
        public bool IsIncrement { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public int AgeMin { get; set; }
        [DataMember]
        public int AgeMax { get; set; }
        [DataMember]
        public int BoughtMin { get; set; }
        [DataMember]
        public int BoughtMax { get; set; }
        [DataMember]
        public int MonthStart { get; set; }
        [DataMember]
        public int MonthEnd { get; set; }
        [DataMember]
        public int DayStart  { get; set; }
        [DataMember]
        public int DayEnd { get; set; }
        [DataMember]
        public string WeekDays { get; set; }

        [DataMember]
        public int Value
        {
            get
            {
                int nmb;
                int.TryParse(ValueText.Replace("%","").Trim(), out nmb);
                return (ValueText.IndexOf("%") > 0) ? nmb : nmb*100;
            }
            set { ValueText = (value>100) ? (value/100).ToString() : value.ToString()+"%"; }
        }
        [DataMember]
        public List<int> PackageIds { get; set; }
        [DataMember]
        public string ValueText { get; set; }
        public string Requirements { get; set; }
        public bool Sunday
        {
            get { return IsDiscountThisDay(0); } 
            set { WeekDays=WeekDays.Remove(0, 1).Insert(0, (value) ? "1" : "0"); }
        }
        public bool Monday
        {
            get { return IsDiscountThisDay(1); } 
            set { WeekDays=WeekDays.Remove(1, 1).Insert(1, (value) ? "1" : "0"); }
        }
        public bool Tuesday
        {
            get { return IsDiscountThisDay(2); } 
            set { WeekDays=WeekDays.Remove(2, 1).Insert(2, (value) ? "1" : "0"); }
        }
        public bool Wednesday
        {
            get { return IsDiscountThisDay(3); } 
            set { WeekDays=WeekDays.Remove(3, 1).Insert(3, (value) ? "1" : "0"); }
        }
        public bool Thursday
        {
            get { return IsDiscountThisDay(4); } 
            set { WeekDays=WeekDays.Remove(4, 1).Insert(4, (value) ? "1" : "0"); }
        }
        public bool Friday
        {
            get { return IsDiscountThisDay(5); } 
            set { WeekDays=WeekDays.Remove(5, 1).Insert(5, (value) ? "1" : "0"); }
        }
        public bool Saturday
        {
            get { return IsDiscountThisDay(6); } 
            set { WeekDays=WeekDays.Remove(6, 1).Insert(6, (value) ? "1" : "0"); }
        }
        
        public Discount()
        {
            PackageIds=new List<int>();
        }
        public Discount CopyInstance()
        {
            Discount discount = new Discount();
            discount.Name = "[КОПИЯ] " + Name;
            discount.IsGlobal = IsGlobal;
            discount.Gender = Gender;
            discount.Code = Code;
            discount.AgeMin = AgeMin;
            discount.AgeMax = AgeMax;
            discount.BoughtMin = BoughtMin;
            discount.BoughtMax = BoughtMax;
            discount.DayStart = DayStart;
            discount.DayEnd = DayEnd;
            discount.WeekDays = WeekDays;
            discount.Value = Value;
            return discount;
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (Value <= 0) em.Add(new ResultMessage(2, "Размер скидки:", OperationErrors.ZeroNumber));
            return em;
        }

        public bool IsDiscountThisDay(int day)
        {
            return WeekDays.Substring(day, 1) == "1";
        }
    }
}
