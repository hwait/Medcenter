using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medcenter.Desktop.Infrastructure
{
    public static class Utils
    {
        public static Dictionary<int, string> MonthsDictionary
        {
            get
            {
                return new Dictionary<int, string>
                {
                    {0,"Январь"},
                    {1,"Февраль"},
                    {2,"Март"},
                    {3,"Апрель"},
                    {4,"Май"},
                    {5,"Июнь"},
                    {6,"Июль"},
                    {7,"Август"},
                    {8,"Сентябрь"},
                    {9,"Октябрь"},
                    {10,"Ноябрь"},
                    {11,"Декабрь"}
                };
            }
        }
        public static T Clone<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static int TimerShowMessage = 4;
        public static int MinimalDuration = 5;

        public static string GetUserFotoPath(int userId)
        {
            return string.Format("{0}Fotos\\{1}.jpg", AppDomain.CurrentDomain.BaseDirectory, userId);
        }
        public static string GetUserFotoPath(string file)
        {
            return string.Format("{0}Fotos\\{1}", AppDomain.CurrentDomain.BaseDirectory, file);
        }
    }
}
