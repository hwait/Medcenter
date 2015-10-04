using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Misc;

namespace Medcenter.Service.Model.Types
{
    public class Formula
    {
        Dictionary<string,decimal> FieldsAffected { get; set; }
        Phrase Target { get; set; }

        public void Calc(string key, decimal value)
        {
            if (!FieldsAffected.ContainsKey(key)) return;
            FieldsAffected[key] = value;
            string[] separator = {" | "};
            if (!FieldsAffected.ContainsValue(0))
                Target.V1 = Calculations.Calc(Target.PositionName.Split(separator, StringSplitOptions.None)[0].Trim(), FieldsAffected) / 100;
        }

        public Formula(Phrase phrase)
        {
            FieldsAffected=new Dictionary<string, decimal>();
            string[] separator = {" | "},list;
            list = phrase.PositionName.Split(separator, StringSplitOptions.None);
            for (int i = 1; i < list.Length; i++)
            {
                var l = list[i];
                FieldsAffected.Add(l.Trim(), 0);
            }
            Target = phrase;
        }
    }
}
