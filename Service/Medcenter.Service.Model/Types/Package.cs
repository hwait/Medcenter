using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Interfaces;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Package : IItem
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int PackageGroupId { get; set; }
        [DataMember]
        public int DoctorId { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public List<int> PackageGroupIds { get; set; }
        [DataMember]
        public List<int> DoctorIds { get; set; }
        public bool IsChanged { get; set; }
        public bool IsRemoved { get; set; }

        public Package()
        {
            PackageGroupIds=new List<int>();
        }
        public Package CopyInstance()
        {
            Package Package=new Package();
            Package.Name = "[КОПИЯ] "+Name;
            Package.ShortName = ShortName;
            Package.Duration = Duration;
            Package.Cost = Cost;
            return Package;
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(ShortName)) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.EmptyString));
            if (Cost<=0) em.Add(new ResultMessage(2, "Стоимость:", OperationErrors.ZeroNumber));
            if (Duration <= 0) em.Add(new ResultMessage(2, "Длительность:", OperationErrors.ZeroNumber));
            return em;
        }
    }
}
