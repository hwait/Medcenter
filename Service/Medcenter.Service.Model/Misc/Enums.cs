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
    public enum DecorationTypes : int
    {
        // 0 - in text, 1 - first paragraph, 2 - 1 + last paragraph, +10 - with Position Name
        HeaderOnly=0,
        InText = 1,
        StartsWithNewParagraph = 2,
        StartsAndEndsWithNewParagraph = 3,
        InTextWithPosition = 11,
        StartsWithNewParagraphWithPosition = 12,
        StartsAndEndsWithNewParagraphWithPosition = 13
    }
    public enum PhraseTypes : int
    {
        Header=0,
        String = 1,
        Number = 2,
        Formula = 3,
        Drug = 4,
        Picture = 5
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
