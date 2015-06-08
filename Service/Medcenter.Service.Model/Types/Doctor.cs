using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Doctor
    {
        public Doctor(int id, string name, string color, bool isActive)
        {
            Id = id;
            Name = name;
            Color = color;
            IsActive = isActive;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        //var color =  System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
    }
}
