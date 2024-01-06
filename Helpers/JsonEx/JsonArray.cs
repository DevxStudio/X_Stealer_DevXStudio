// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers.JsonEx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Helpers;

    public class JsonArray : JsonValue, IList<JsonValue>, ICollection<JsonValue>, IEnumerable<JsonValue>, IEnumerable
    {
        private readonly List<JsonValue> list;
        public override int Count => list.Count;
        public bool IsReadOnly => false;
        public sealed override JsonValue this[int index] { get => list[index]; set => list[index] = value; }
        public override Enums.JsonType JsType => Enums.JsonType.Array;
        public JsonArray(params JsonValue[] items)
        {
            list = new List<JsonValue>();
            AddRange(items);
        }
        public JsonArray(IEnumerable<JsonValue> items) => list = items != null ? new List<JsonValue>(items) : throw new ArgumentNullException("items");
        public void Add(JsonValue item)
        {
            if (item == null) { throw new ArgumentNullException("item"); }
            list.Add(item);
        }
        public void AddRange(IEnumerable<JsonValue> items)
        {
            if (items == null) { throw new ArgumentNullException("items"); }
            list.AddRange(items);
        }
        public void AddRange(params JsonValue[] items)
        {
            if (items != null)
            {
                list.AddRange(items);
            }
        }
        public void Clear() => list?.Clear();
        public bool Contains(JsonValue item) => list.Contains(item);
        public void CopyTo(JsonValue[] array, int arrayIndex) => list?.CopyTo(array, arrayIndex);
        public int IndexOf(JsonValue item) => list.IndexOf(item);
        public void Insert(int index, JsonValue item) => list.Insert(index, item);
        public bool Remove(JsonValue item) => list.Remove(item);
        public void RemoveAt(int index) => list?.RemoveAt(index);
        public override void Save(Stream stream, bool parsing)
        {
            if (stream != null)
            {
                stream.WriteByte(91);
                for (int i = 0; i < list.Count; i++)
                {
                    JsonValue jsonValue = list[i];
                    if (jsonValue != null)
                    {
                        jsonValue.Save(stream, parsing);
                    }
                    else
                    {
                        stream.WriteByte(110);
                        stream.WriteByte(117);
                        stream.WriteByte(108);
                        stream.WriteByte(108);
                    }
                    if (i < Count - 1)
                    {
                        stream.WriteByte(44);
                        stream.WriteByte(32);
                    }
                }
                stream.WriteByte(93);
            }
            else
            {
                throw new ArgumentNullException("stream");
            }
        }
        IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
    }
}