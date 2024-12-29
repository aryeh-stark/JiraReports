using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Structs;

namespace JiraReportsClient.Utils.Json.Converters;

public class FixedDoubleJsonConverter : JsonConverter<FixedDouble>
{
    // Serialize FixedDouble as a double
    public override void Write(Utf8JsonWriter writer, FixedDouble value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((double)value); // Convert to double for serialization
    }

    // Deserialize double into FixedDouble
    public override FixedDouble Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected a numeric value.");
        }

        double rawValue = reader.GetDouble();
        return new FixedDouble(rawValue); // Convert double to FixedDouble
    }
}