using Newtonsoft.Json;
using System;

namespace Monads.DataOps.Json
{
    public class ClearableValueJsonConverter<T> : JsonConverter<ClearableValue<T>>
    {
        private readonly JsonConverter<T> _valueConverter;

        public ClearableValueJsonConverter(
            JsonConverter<T> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override ClearableValue<T> ReadJson(JsonReader reader, Type objectType, ClearableValue<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            EnsureValueConverter(serializer);

            var builder = new ClearableValueBuilder<T>();
            if (reader.TokenType == JsonToken.StartObject)
            {
                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TryReadPropertyAs(Constants.StatePropertyName, serializer, e => e.ReadAsString(), out var stateValue))
                        builder.State = stateValue;
                    else if (reader.TryDeserializeProperty(Constants.ValuePropertyName, serializer, out T valueObj))
                        builder.Value = valueObj;
                }
            }

            if (!builder.TryBuild(out var clearable))
                throw new InvalidOperationException($"Could not read {typeof(ClearableValue<T>)} from JSON.");

            return clearable;
        }

        public override void WriteJson(JsonWriter writer, ClearableValue<T> clearable, JsonSerializer serializer)
        {
            EnsureValueConverter(serializer);

            writer.WriteStartObject();

            string state = clearable.Match(
                set: _ => Constants.Clearable_SetState,
                clear: () => Constants.Clearable_ClearState,
                noAction: () => Constants.Clearable_NoActionState);
            writer.WritePropertyName(Constants.StatePropertyName);
            writer.WriteValue(state);

            var (isSet, value) = clearable.Match(
                set: e => (true, e),
                clear: () => (false, default(T)),
                noAction: () => (false, default(T)));
            if (isSet)
            {
                writer.WritePropertyName(Constants.ValuePropertyName);
                serializer.Serialize(writer, value);
            }

            writer.WriteEndObject();
        }

        private void EnsureValueConverter(JsonSerializer serializer)
        {
            if (_valueConverter != null && !serializer.Converters.Contains(_valueConverter))
                serializer.Converters.Add(_valueConverter);
        }
    }
}
