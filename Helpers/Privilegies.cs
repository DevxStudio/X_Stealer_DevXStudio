// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System.Security.Principal;

    public static class Privilegies
    {
        /// <summary>
        /// <b><br>~[RUS]~</br></b><br>Метод для проверки прав Администратора</br>
        /// <b><br>~[ENG]~</br></b><br>Method for checking Administrator rights</br>
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                try
                {
                    using var identity = WindowsIdentity.GetCurrent();
                    if (null != identity)
                    {
                        var principal = new WindowsPrincipal(identity);
                        return principal.IsInRole(WindowsBuiltInRole.Administrator);
                    }
                }
                catch { }
                return false;
            }
        }

        // public static bool IsElevated => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }
}