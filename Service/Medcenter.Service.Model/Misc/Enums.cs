using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Misc
{
    public enum Weekdays : int
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }
    public enum ReceptopnStatuses : byte
    {
        Empty = 0,
        Enlisted = 1,
        Confirmed = 2,
        Paid = 3,
        InProcess = 4,
        Done = 5
    }
}
