// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers.JsonEx
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Helpers;

    public class JsonPrimitive : JsonValue
    {
        private static readonly byte[] true_bytes = GetBytes_UTF("true"), false_bytes = GetBytes_UTF("false");
        private static byte[] GetBytes_UTF(string value)
        {
            byte[] result;
            try
            {
                result = Encoding.UTF8.GetBytes(value);
            }
            catch { result = Encoding.Default.GetBytes(value); }
            return result;
        }
        public object Value { get; }
        public override Enums.JsonType JsType => Value != null ? ((Type.GetTypeCode(Value.GetType())) switch
        {
            TypeCode.Boolean => Enums.JsonType.Boolean,
            TypeCode.String => Enums.JsonType.String,
            _ => Enums.JsonType.Number,
        }) : Enums.JsonType.String;
        #region Constuctors
        public JsonPrimitive(bool value) => Value = value;
        public JsonPrimitive(byte value) => Value = value;
        public JsonPrimitive(char value) => Value = value;
        public JsonPrimitive(decimal value) => Value = value;
        public JsonPrimitive(double value) => Value = value;
        public JsonPrimitive(float value) => Value = value;
        public JsonPrimitive(int value) => Value = value;
        public JsonPrimitive(long value) => Value = value;
        public JsonPrimitive(sbyte value) => Value = value;
        public JsonPrimitive(short value) => Value = value;
        public JsonPrimitive(string value) => Value = value;
        public JsonPrimitive(DateTime value) => Value = value;
        public JsonPrimitive(uint value) => Value = value;
        public JsonPrimitive(ulong value) => Value = value;
        public JsonPrimitive(ushort value) => Value = value;
        public JsonPrimitive(DateTimeOffset value) => Value = value;
        public JsonPrimitive(Guid value) => Value = value;
        public JsonPrimitive(TimeSpan value) => Value = value;
        public JsonPrimitive(Uri value) => Value = value;
        public JsonPrimitive(object value) => Value = value;
        public JsonPrimitive() { }
        #endregion
        public override void Save(Stream stream, bool parsing)
        {
            switch (JsType)
            {
                case Enums.JsonType.Boolean: stream.Write((bool)Value ? true_bytes : false_bytes, 0, (bool)Value ? 4 : 5); break;
                case Enums.JsonType.String:
                    {
                        stream.WriteByte(34);
                        byte[] bytes = Encoding.UTF8.GetBytes(EscapeString(Value?.ToString()));
                        stream.Write(bytes, 0, bytes.Length);
                        stream.WriteByte(34); break;
                    }
                default:
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(GetFormattedString());
                        stream.Write(bytes, 0, bytes.Length); break;
                    }
            }
        }
        public string GetFormattedString()
        {
            switch (JsType)
            {
                case Enums.JsonType.String:
                    if (!(Value is string) && Value != null)
                    {
                        return Value is char ? Value?.ToString() : throw new NotImplementedException($"GetFormattedString from value type {Value?.GetType()}");
                    }
                    string text2 = Value as string;
                    return string.IsNullOrEmpty(text2) ? "null" : (text2?.Trim('"'));
                case Enums.JsonType.Number:
                    {
                        string text = (!(Value is float) && !(Value is double)) ? ((IFormattable)Value).ToString("G", NumberFormatInfo.InvariantInfo) : ((IFormattable)Value).ToString("R", NumberFormatInfo.InvariantInfo);
                        return text == "NaN" || text == "Infinity" || text == "-Infinity" ? $"\"{text}\"" : text;
                    }
                default: throw new InvalidOperationException();
            }
        }
    }
}