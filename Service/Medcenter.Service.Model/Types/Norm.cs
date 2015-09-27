using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Norm
    {
        [DataMember]
        public int ValueMin { get; set; }
        [DataMember]
        public int ValueMax { get; set; }
        [DataMember]
        public string Name { get; set; }

        public string Output
        {
            get
            {
                decimal vmin = ((decimal)ValueMin) / 100;
                decimal vmax = ((decimal)ValueMax) / 100;
                if (ValueMin == 0)
                {
                    vmax = ((decimal) ValueMax+1) / 100;
                    return string.Format(", {0} (<{1})", Name, vmax);
                }
                if (ValueMax == 1000000)
                {
                    vmin = ((decimal)ValueMin-1) / 100;
                    return string.Format(", {0} (>{1})", Name, vmin);
                }
                return string.Format(", {0} ({1}-{2})", Name, vmin, vmax);
            }
        }

    }
}
