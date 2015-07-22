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
        public DateTime? BirthDate { get; set; }
        [DataMember]
        public bool? Gender { get; set; }
        [DataMember]
        public int CityId { get; set; }
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

        public Patient()
        {
            Receptions=new List<Reception>();
            Contacts=new Dictionary<string, string>();
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Surname)) em.Add(new ResultMessage(2, "Фамилия:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(FirstName)) em.Add(new ResultMessage(2, "Имя:", OperationErrors.EmptyString));
            if (BirthDate == null) em.Add(new ResultMessage(2, "Дата рождения:", OperationErrors.VariantNotChosen));
            if (Gender == null) em.Add(new ResultMessage(2, "Пол:", OperationErrors.VariantNotChosen));
            if (CityId<1) em.Add(new ResultMessage(2, "Город:", OperationErrors.VariantNotChosen));
            if (string.IsNullOrEmpty(PhoneNumber) || (string.IsNullOrEmpty(MobileCode) || string.IsNullOrEmpty(MobileNumber))) em.Add(new ResultMessage(2, "Телефон:", OperationErrors.EmptyString));
            return em;
        }
    }
}
