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
    public class InspectionGroup
    {
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public byte Row { get; set; }

        [DataMember]
        public string Color
        {
            get; 
            set;
        }
        [DataMember]
        public List<int> InspectionIds { get; set; }

        public Color CurrentColor
        {
            get { return ColorTranslator.FromHtml(Color); }
            set { Color=ColorTranslator.ToHtml(value); }
        }
        public bool IsChanged { get; set; }

        public bool IsRemoved { get; set; }
        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            if (string.IsNullOrEmpty(Name)) em.Add(new ResultMessage(2, "Наименование:", OperationErrors.EmptyString));
            if (string.IsNullOrEmpty(ShortName)) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.EmptyString));
            else if (ShortName.Length != 3) em.Add(new ResultMessage(2, "Краткое наименование:", OperationErrors.StringLengthNotEqual3));
            //if (string.IsNullOrEmpty(Color)) em.Add(new ResultMessage(2, "Пароль:", OperationErrors.EmptyString));
            return em;
        }
    }
   
}
