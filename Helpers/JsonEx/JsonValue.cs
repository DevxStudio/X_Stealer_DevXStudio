// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers.JsonEx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Helpers;

    public abstract class JsonValue : IEnumerable
    {
        protected JsonValue() { }

        public virtual int Count => throw new InvalidOperationException();
        public abstract Enums.JsonType JsType { get; }
        public virtual JsonValue this[int index]
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }
        public virtual JsonValue this[string key]
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }
        public static JsonValue Load(Stream stream) => stream != null ? Load(new StreamReader(stream, detectEncodingFromByteOrderMarks: true)) : throw new ArgumentNullException("stream");
        public static JsonValue Load(TextReader textReader) => textReader != null ? ToJsonValue(new JavaScriptReader(textReader).Read()) : throw new ArgumentNullException("textReader");
        private static IEnumerable<KeyValuePair<string, JsonValue>> ToJsonPairEnumerable(IEnumerable<KeyValuePair<string, object>> kvpc) => kvpc?.Select(item => new KeyValuePair<string, JsonValue>(item.Key, ToJsonValue(item.Value)));
        private static IEnumerable<JsonValue> ToJsonValueEnumerable(IEnumerable arr) => from object item in arr select ToJsonValue(item);
        public static JsonValue ToJsonValue<T>(T ret)
        {
            if (ret != null)
            {
                T val;
                switch (val = ret)
                {
                    case bool _: return new JsonPrimitive((bool)(object)val);
                    case byte _: return new JsonPrimitive((byte)(object)val);
                    case uint _: return new JsonPrimitive((uint)(object)val);
                    case char _: return new JsonPrimitive((char)(object)val);
                    case decimal _: return new JsonPrimitive((decimal)(object)val);
                    case double _: return new JsonPrimitive((double)(object)val);
                    case float _: return new JsonPrimitive((float)(object)val);
                    case int _: return new JsonPrimitive((int)(object)val);
                    case long _: return new JsonPrimitive((long)(object)val);
                    case sbyte _: return new JsonPrimitive((sbyte)(object)val);
                    case short _: return new JsonPrimitive((short)(object)val);
                    case ulong _: return new JsonPrimitive((ulong)(object)val);
                    case ushort _: return new JsonPrimitive((ushort)(object)val);
                    case DateTime _: return new JsonPrimitive((DateTime)(object)val);
                    case DateTimeOffset _: return new JsonPrimitive((DateTimeOffset)(object)val);
                    case Guid _: return new JsonPrimitive((Guid)(object)val);
                    case TimeSpan _: return new JsonPrimitive((TimeSpan)(object)val);
                    default: break;
                }
                if (ret is string value11) { return new JsonPrimitive(value11); }
                switch (ret)
                {
                    case Uri value19: return new JsonPrimitive(value19);
                    case IEnumerable<KeyValuePair<string, object>> enumerable: return new JsonObject(ToJsonPairEnumerable(enumerable));
                    case IEnumerable enumerable2: return new JsonArray(ToJsonValueEnumerable(enumerable2));
                    default: break;
                }
                if (!(ret is IEnumerable))
                {
                    var dictionary = new Dictionary<string, object>();
                    foreach (PropertyInfo propertyInfo in ret?.GetType()?.GetProperties())
                    {
                        dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(ret, null));
                    }
                    if (dictionary.Count > 0) { return new JsonObject(ToJsonPairEnumerable(dictionary)); }
                }
                throw new NotSupportedException($"Unexpected parser return type: {ret?.GetType()}");
            }
            return null;
        }
        public static JsonValue Parse(string jsonString) => jsonString != null ? Load(new StringReader(jsonString)) : throw new ArgumentNullException("jsonString");
        public virtual bool ContainsKey(string key) => throw new InvalidOperationException();
        public virtual void Save(Stream stream, bool parsing)
        {
            if (stream != null)
            {
                Save(new StreamWriter(stream), parsing);
            }
            else
            {
                throw new ArgumentNullException("stream");
            }
        }
        public virtual void Save(TextWriter textWriter, bool parsing)
        {
            if (textWriter != null) { Savepublic(textWriter, parsing); }
            else { throw new ArgumentNullException("textWriter"); }
        }
        private void Savepublic(TextWriter w, bool saving)
        {
            switch (JsType)
            {
                case Enums.JsonType.Object:
                    {
                        w.Write('{');
                        bool flag = false;
                        foreach (KeyValuePair<string, JsonValue> item in (JsonObject)this)
                        {
                            if (flag) { w.Write(", "); }
                            w.Write('"');
                            w.Write(EscapeString(item.Key));
                            w.Write("\": ");
                            if (item.Value == null) { w.Write("null"); }
                            else { item.Value.Savepublic(w, saving); }
                            flag = true;
                        }
                        w.Write('}');
                        break;
                    }
                case Enums.JsonType.Array:
                    {
                        w.Write('[');
                        bool flag = false;
                        foreach (JsonValue item2 in (JsonArray)this)
                        {
                            if (flag)
                            {
                                w.Write(", ");
                            }
                            if (item2 != null)
                            {
                                item2.Savepublic(w, saving);
                            }
                            else
                            {
                                w.Write("null");
                            }
                            flag = true;
                        }
                        w.Write(']');
                        break;
                    }
                case Enums.JsonType.Boolean: w.Write(this ? "true" : "false"); break;
                case Enums.JsonType.String:
                    if (saving) { w.Write('"'); }
                    w.Write(EscapeString(((JsonPrimitive)this).GetFormattedString()));
                    if (saving) { w.Write('"'); }
                    break;
                default: w.Write(((JsonPrimitive)this).GetFormattedString()); break;
            }
        }
        public string ToString(bool saving = true)
        {
            using var writer = new StringWriter();
            Save(writer, saving);
           // writer.Flush();
            return writer?.ToString();
        }
        IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException();
        private bool NeedEscape(string src, int i)
        {
            char c = src[i];
            return c < ' '
                   || c == '"'
                   || c == '\\'
                   || (c >= '\ud800' && c <= '\udbff' && (i == src.Length - 1 || src[i + 1] < '\udc00' || src[i + 1] > '\udfff'))
                   || (c >= '\udc00' && c <= '\udfff' && (i == 0 || src[i - 1] < '\ud800' || src[i - 1] > '\udbff'))
                   || c == '\u2028'
                   || c == '\u2029'
                   || (c == '/' && i > 0 && src[i - 1] == '<');
        }
        public string EscapeString(string src)
        {
            if (src != null)
            {
                for (int i = 0; i < src.Length; i++)
                {
                    if (!NeedEscape(src, i)) { continue; }
                    var stringBuilder = new StringBuilder();
                    if (i > 0)
                    {
                        stringBuilder.Append(src, 0, i);
                    }
                    return DoEscapeString(stringBuilder ?? null, src, i);
                }
                return src;
            }
            return null;
        }
        private string DoEscapeString(StringBuilder sb, string src, int cur)
        {
            int num = cur;
            try
            {
                for (int i = cur; i < src.Length; i++)
                {
                    if (NeedEscape(src, i))
                    {
                        sb.Append(src, num, i - num);
                        switch (src[i])
                        {
                            case '\b': sb.Append("\\b"); break;
                            case '\f': sb.Append("\\f"); break;
                            case '\n': sb.Append("\\n"); break;
                            case '\r': sb.Append("\\r"); break;
                            case '\t': sb.Append("\\t"); break;
                            case '"': sb.Append("\\\""); break;
                            case '\\': sb.Append("\\\\"); break;
                            case '/': sb.Append("\\/"); break;
                            default: sb.Append("\\u"); sb.Append(((int)src[i]).ToString("x04")); break;
                        }
                        num = i + 1;
                    }
                }
            }
            catch { }
            sb.Append(src, num, src.Length - num);
            return sb?.ToString();
        }
        #region JsonPrimitive
        public static implicit operator JsonValue(bool value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(byte value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(char value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(decimal value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(double value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(float value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(int value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(long value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(sbyte value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(short value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(string value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(uint value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(ulong value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(ushort value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(DateTime value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(DateTimeOffset value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(Guid value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(TimeSpan value) => new JsonPrimitive(value);
        public static implicit operator JsonValue(Uri value) => new JsonPrimitive(value);
        public static implicit operator bool(JsonValue value) => value != null ? Convert.ToBoolean(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator byte(JsonValue value) => value != null ? Convert.ToByte(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator char(JsonValue value) => value != null ? Convert.ToChar(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator decimal(JsonValue value) => value != null ? Convert.ToDecimal(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator double(JsonValue value) => value != null ? Convert.ToDouble(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator float(JsonValue value) => value != null ? Convert.ToSingle(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator int(JsonValue value) => value != null ? Convert.ToInt32(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator long(JsonValue value) => value != null ? Convert.ToInt64(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator sbyte(JsonValue value) => value != null ? Convert.ToSByte(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator short(JsonValue value) => value != null ? Convert.ToInt16(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator string(JsonValue value) => value?.ToString();
        public static implicit operator uint(JsonValue value) => value != null ? Convert.ToUInt32(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator ulong(JsonValue value) => value != null ? Convert.ToUInt64(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator ushort(JsonValue value) => value != null ? Convert.ToUInt16(((JsonPrimitive)value)?.Value, NumberFormatInfo.InvariantInfo) : throw new ArgumentNullException("value");
        public static implicit operator DateTime(JsonValue value) => value != null ? (DateTime)((JsonPrimitive)value)?.Value : throw new ArgumentNullException("value");
        public static implicit operator DateTimeOffset(JsonValue value) => value != null ? (DateTimeOffset)((JsonPrimitive)value)?.Value : throw new ArgumentNullException("value");
        public static implicit operator TimeSpan(JsonValue value) => value != null ? (TimeSpan)((JsonPrimitive)value)?.Value : throw new ArgumentNullException("value");
        public static implicit operator Guid(JsonValue value) => value != null ? (Guid)((JsonPrimitive)value)?.Value : throw new ArgumentNullException("value");
        public static implicit operator Uri(JsonValue value) => value != null ? (Uri)((JsonPrimitive)value)?.Value : throw new ArgumentNullException("value");
        #endregion
    }
}