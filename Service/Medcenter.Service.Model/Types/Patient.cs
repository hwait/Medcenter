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
    public class Patient
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string SecondName { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public bool? Gender { get; set; }
        [DataMember]
        public City City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string MobileCode { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public Dictionary<string,string> Contacts { get; set; }
        [DataMember]
        public List<Reception> Receptions { get; set; }
        public string PhoneCode { get; set; }
        public string Name
        {
            get
            {
                if (BirthDate == DateTime.MinValue) return "Пациент не выбран.";
                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate > today.AddYears(-age)) age--;
                string name = (age < 3) ? 
                      string.Format("{0} {1} {2}, {3} мес. ", Surname, FirstName, SecondName, age * 12 + BirthDate.Month) 
                    : string.Format("{0} {1} {2}, {3} лет ", Surname, FirstName, SecondName, age);
                return name;
            }
        }

        public Patient()
        {
            Receptions=new List<Reception>();
            Contacts=new Dictionary<string, string>();
            BirthDate=DateTime.MinValue;
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Surname)) em.Add(new ResultMessage(2, "Фамилия:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(FirstName)) em.Add(new ResultMessage(2, "Имя:", OperationErrors.EmptyString));
            if (BirthDate == DateTime.MinValue) em.Add(new ResultMessage(2, "Дата рождения:", OperationErrors.VariantNotChosen));
            if (Gender == null) em.Add(new ResultMessage(2, "Пол:", OperationErrors.VariantNotChosen));
            if (City==null) em.Add(new ResultMessage(2, "Город:", OperationErrors.VariantNotChosen));
            if (string.IsNullOrEmpty(PhoneNumber) || (string.IsNullOrEmpty(MobileCode) || string.IsNullOrEmpty(MobileNumber))) em.Add(new ResultMessage(2, "Телефон:", OperationErrors.EmptyString));
            return em;
        }
    }
}
