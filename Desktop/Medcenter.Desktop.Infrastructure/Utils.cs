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
        public static T Clone<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static int TimerShowMessage = 4;

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
