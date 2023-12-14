using System;
using Newtonsoft.Json;

namespace Monads.DataOps.Json;

internal static class JsonReaderExtensions
{
    public static bool TryGetPropertyName(this JsonReader reader, out string propertyName)
    {
        if (reader.TokenType == JsonToken.PropertyName
            && reader.ValueType == typeof(string))
        {
            propertyName = (string)reader.Value;
            return true;
        }

        propertyName = null;
        return false;
    }

    public static bool TryReadPropertyAsNullable<T>(this JsonReader reader, string name, JsonSerializer serializer, Func<JsonReader, T?> read, out T result)
        where T : struct
    {
        if (reader.TryGetPropertyName(out string propertyName)
            && string.Equals(propertyName, name, StringComparison.OrdinalIgnoreCase))
        {
            var value = read(reader);
            result = value ?? default(T);
            return value != null;
        }

        result = default;
        return false;
    }

    public static bool TryReadPropertyAs<T>(this JsonReader reader, string name, JsonSerializer serializer, Func<JsonReader, T> read, out T result)
        where T : class
    {
        if (reader.TryGetPropertyName(out string propertyName)
            && string.Equals(propertyName, name, StringComparison.OrdinalIgnoreCase))
        {
            result = read(reader);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryDeserializeProperty<T>(this JsonReader reader, string name, JsonSerializer serializer, out T result)
    {
        if (reader.TryGetPropertyName(out string propertyName)
            && string.Equals(propertyName, name, StringComparison.OrdinalIgnoreCase)
            && reader.Read())
        {
            result = serializer.Deserialize<T>(reader);
            return true;
        }

        result = default;
        return false;
    }
}