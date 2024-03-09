using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Gameshow.Shared.Events.Base;

public class InterfaceConverter<T> : JsonConverter<T>
    where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader readerClone = reader;
        if (readerClone.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        string propertyName = readerClone.GetString();
        if (propertyName != "$type")
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string typeValue = readerClone.GetString();
        var instance = Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, typeValue).Unwrap();
        var entityType = instance.GetType();

        var deserialized = JsonSerializer.Deserialize(ref reader, entityType, options);
        return (T)deserialized;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (T)null, options);
                break;
            default:
                var type = value.GetType();
                using (var jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options)))
                {
                    writer.WriteStartObject();
                    writer.WriteString("$type", type.FullName);

                    foreach (var element in jsonDocument.RootElement.EnumerateObject())
                    {
                        element.WriteTo(writer);
                    }

                    writer.WriteEndObject();
                }
                break;
        }
    }
}