using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Modules.CabinetModule.Model
{
    public class Calculus
    {
        public List<Formula> Formulas { get; set; }
        public void TryToCalc(string key, decimal val)
        {
            foreach (var formula in Formulas)
            {
                formula.Calc(key.Trim(),val);
            }
        }

        public void AddFormula(Phrase phrase)
        {
            Formulas.Add(new Formula(phrase));
        }
        public Calculus()
        {
            Formulas=new List<Formula>();
        }
    }
}
