using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace JiraReportsClient.Entities.Issues.Atlassian.Json;

public class JiraNullableDateTimeConverter : JsonConverter<DateTime?>
{
    private static readonly Regex OffsetPattern = new(@"\+(\d{2})(\d{2})$");

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        var raw = reader.GetString();
        if (string.IsNullOrWhiteSpace(raw)) return null;

        raw = OffsetPattern.Replace(raw, "+$1:$2");

        // Return null if parse fails
        return DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dt)
            ? dt
            : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}