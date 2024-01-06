// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers.JsonEx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Helpers;

    public class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<KeyValuePair<string, JsonValue>>, IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable
    {
        private readonly SortedDictionary<string, JsonValue> map;
        public override int Count => map.Count;
        public sealed override JsonValue this[string key] { get => map[key]; set => map[key] = value; }
        public override Enums.JsonType JsType => Enums.JsonType.Object;
        public ICollection<string> Keys => map?.Keys;
        public ICollection<JsonValue> Values => map?.Values;
        bool ICollection<KeyValuePair<string, JsonValue>>.IsReadOnly => false;
        public JsonObject(params KeyValuePair<string, JsonValue>[] items)
        {
            map = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);
            if (items != null) { AddRange(items); }
        }
        public JsonObject(IEnumerable<KeyValuePair<string, JsonValue>> items)
        {
            if (items != null)
            {
                map = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);
                AddRange(items);
            }
            else { throw new ArgumentNullException("items"); }
        }
        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator() => map?.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => map?.GetEnumerator();
        public void Add(string key, JsonValue value)
        {
            if (key != null)
            {
                map.Add(key, value);
            }
            else { throw new ArgumentNullException("key"); }
        }
        public void Add(KeyValuePair<string, JsonValue> pair) => Add(pair.Key, pair.Value);
        public void AddRange(IEnumerable<KeyValuePair<string, JsonValue>> items)
        {
            if (items != null)
            {
                foreach (KeyValuePair<string, JsonValue> item in items)
                {
                    map.Add(item.Key, item.Value);
                }
            }
            else
            {
                throw new ArgumentNullException("items");
            }
        }
        public void AddRange(params KeyValuePair<string, JsonValue>[] items) => AddRange((IEnumerable<KeyValuePair<string, JsonValue>>)items);
        public void Clear() => map?.Clear();
        bool ICollection<KeyValuePair<string, JsonValue>>.Contains(KeyValuePair<string, JsonValue> item) => ((ICollection<KeyValuePair<string, JsonValue>>)map).Contains(item);
        bool ICollection<KeyValuePair<string, JsonValue>>.Remove(KeyValuePair<string, JsonValue> item) => ((ICollection<KeyValuePair<string, JsonValue>>)map).Remove(item);
        public override bool ContainsKey(string key) => key == null ? throw new ArgumentNullException("key") : map.ContainsKey(key);
        public void CopyTo(KeyValuePair<string, JsonValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, JsonValue>>)map).CopyTo(array, arrayIndex);
        public bool Remove(string key) => key != null ? map.Remove(key) : throw new ArgumentNullException("key");
        public override void Save(Stream stream, bool parsing)
        {
            if (stream == null) { throw new ArgumentNullException("stream"); }
            stream.WriteByte(123);
            foreach (KeyValuePair<string, JsonValue> item in map)
            {
                stream.WriteByte(34);
                byte[] bytes = Encoding.UTF8.GetBytes(EscapeString(item.Key));
                stream.Write(bytes, 0, bytes.Length);
                stream.WriteByte(34);
                stream.WriteByte(44);
                stream.WriteByte(32);
                if (item.Value == null)
                {
                    stream.WriteByte(110);
                    stream.WriteByte(117);
                    stream.WriteByte(108);
                    stream.WriteByte(108);
                }
                else
                {
                    item.Value.Save(stream, parsing);
                }
            }
            stream.WriteByte(125);
        }
        public bool TryGetValue(string key, out JsonValue value) => map.TryGetValue(key, out value);
    }
}