using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string ShortName { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int InspectionGroupId { get; set; }
        [DataMember]
        public int Cost { get; set; }
        [DataMember]
        public List<int> InspectionGroupIds { get; set; }
        public bool IsChanged { get; set; }
        public bool IsRemoved { get; set; }

        public Inspection()
        {
            InspectionGroupIds=new List<int>();
        }
        public Inspection CopyInstance()
        {
            Inspection inspection=new Inspection();
            inspection.Name = "[КОПИЯ] "+Name;
            inspection.ShortName = ShortName;
            inspection.Duration = Duration;
            inspection.Cost = Cost;
            return inspection;
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
