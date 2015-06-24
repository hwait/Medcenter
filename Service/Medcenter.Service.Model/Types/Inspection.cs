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
    public class Inspection
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public List<int> PackageIds { get; set; }
        public Inspection()
        {
            PackageIds=new List<int>();
        }
        public Inspection CopyInstance()
        {
            Inspection inspection = new Inspection();
            inspection.Name = "[КОПИЯ] " + Name;
            inspection.Cost = Cost;
            return inspection;
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (Cost<=0) em.Add(new ResultMessage(2, "Стоимость:", OperationErrors.ZeroNumber));
            return em;
        }
    }
}
