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
    public class Payment
    {
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public int Sum { get; set; }
        public Payment()
        {
        }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (Sum==0) em.Add(new ResultMessage(2, "Сумма:", OperationErrors.ZeroNumber));
            return em;
        }
    }
}
