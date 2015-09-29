using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.CabinetModule.Model
{
    public class PrintDocument
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public Patient Patient { get; set; }
        public List<PrintPhrase> PrintPhrases { get; set; }
        public string Signature { get; set; }
    }
    public class PrintPhrase
    {
        public int Type { get; set; }
        public string Text { get; set; }
        public string Header { get; set; }
        public int DecorationType { get; set; }
    }
}
