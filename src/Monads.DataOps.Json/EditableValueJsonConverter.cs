using System;
using Newtonsoft.Json;

namespace Monads.DataOps.Json;

public class EditableValueJsonConverter<T> : JsonConverter<EditableValue<T>>
{
    public override EditableValue<T> ReadJson(JsonReader reader, Type objectType, EditableValue<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var builder = new EditableValueBuilder<T>();
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

        if (!builder.TryBuild(out var editable))
            throw new InvalidOperationException($"Could not read {typeof(EditableValue<T>)} from JSON.");

        return editable;
    }

    public override void WriteJson(JsonWriter writer, EditableValue<T> editable, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        string state = editable.Match(
            update: _ => Constants.Editable_UpdateState,
            noAction: () => Constants.Editable_NoActionState);
        writer.WritePropertyName(Constants.StatePropertyName);
        writer.WriteValue(state);

        var (isUpdate, value) = editable.Match(
            update: e => (true, e),
            noAction: () => (false, default(T)));
        if (isUpdate)
        {
            writer.WritePropertyName(Constants.ValuePropertyName);
            serializer.Serialize(writer, value);
        }

        writer.WriteEndObject();
    }
}