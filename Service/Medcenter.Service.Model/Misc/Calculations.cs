using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Misc
{
    public static class Calculations
    {
        public static decimal Calc(string formulaName, Dictionary<string, decimal> ps)
        {
            decimal result = 0;
            switch (formulaName)
            {
                case "Смещение":
                    decimal vs, vd;
                    ps.TryGetValue("MS", out vs);
                    ps.TryGetValue("MD", out vd);
                    result = (vs - vd) / 2;
                    break;
            }
            return result;
        }
    }
}
