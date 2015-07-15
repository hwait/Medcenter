using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Globalization;

namespace Medcenter.Desktop.Infrastructure
{
    public static class Utils
    {
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
        public static DateTime GetLastDayOfWeek(DateTime dayInWeek)
        {
            int delta = (dayInWeek.DayOfWeek > 0) ? 7 - (int) dayInWeek.DayOfWeek : 0;
            DateTime lastDayInWeek = new DateTime(dayInWeek.AddDays(delta).Year, dayInWeek.AddDays(delta).Month, dayInWeek.AddDays(delta).Day, 23,59,0);
            return lastDayInWeek;
        }
        public static string ReadSetting(string key)
        {
            string result;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                result = "-1";
            }
            return result;
        }

        public static bool SaveSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }
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
