using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Desktop.Infrastructure
{
    public static class Statuses
    {
        static private Dictionary<byte, string> _statusDictionary = new Dictionary<byte, string>
        {
            {0,"Не определено"},
            {1,"Не подтверждено"},
            {2,"Подтверждено"},
            {3,"Оплачено"},
            {4,"Идёт исследование"},
            {5,"Завершено"},
        };

        static public string GetStatus(byte key)
        {
            string value = "";
            _statusDictionary.TryGetValue(key, out value);
            return value;
        }
    }
}
