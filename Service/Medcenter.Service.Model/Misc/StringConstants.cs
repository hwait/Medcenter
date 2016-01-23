using System.Collections.Generic;

namespace Medcenter.Service.Model.Misc
{
    public static class Statuses
    {
        static private readonly Dictionary<byte, string> _statusDictionary = new Dictionary<byte, string>
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
