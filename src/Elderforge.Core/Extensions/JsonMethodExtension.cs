using System.Text.Json;
using Elderforge.Core.Utils;

namespace Elderforge.Core.Extensions;

public static class JsonMethodExtension
{
    public static string ToJson<T>(this T obj, bool formatted = true) =>
        JsonSerializer.Serialize(obj, JsonUtils.GetDefaultJsonSettings(formatted));

    public static T FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json, JsonUtils.GetDefaultJsonSettings());

    public static object FromJson(this string json, Type type) => JsonSerializer.Deserialize(json, type, JsonUtils.GetDefaultJsonSettings());
}
