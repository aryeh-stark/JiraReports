using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Utils.Json.Converters;

public class CustomStringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && int.TryParse(reader.GetString(), out var value))
        {
            return value;
        }

        // If it's already a number, just read it
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var number))
        {
            return number;
        }

        throw new JsonException($"Unable to convert {reader.GetString()} to an integer.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        // Write the integer as a number
        writer.WriteNumberValue(value);
    }
}

public class CustomStringToNullableIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && int.TryParse(reader.GetString(), out var value))
        {
            return value;
        }

        // If it's already a number, just read it
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var number))
        {
            return number;
        }

        throw new JsonException($"Unable to convert {reader.GetString()} to an integer.");
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value != null) writer.WriteNumberValue(value.Value);
    }
}