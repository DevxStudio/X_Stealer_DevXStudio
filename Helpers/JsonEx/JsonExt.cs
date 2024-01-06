// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers.JsonEx
{
    using System;
    using System.IO;

    public static class JsonExt
    {
        public static JsonValue FromJSON(this string json) => string.IsNullOrWhiteSpace(json) ? throw new ArgumentException($"{nameof(json)}") : JsonValue.Load(new StringReader(json));

        public static string ToJSON<T>(this T instance) => JsonValue.ToJsonValue(instance) ?? null;
    }
}