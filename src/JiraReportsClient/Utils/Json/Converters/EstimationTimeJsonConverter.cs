using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;

public class EstimationTimeJsonConverter : JsonConverter<EstimationTime>
{
    // Serialize EstimationTime as the time in seconds
    public override void Write(Utf8JsonWriter writer, EstimationTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Seconds); // Serialize as seconds
    }

    // Deserialize a numeric value into an EstimationTime
    public override EstimationTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected a numeric value for EstimationTime.");
        }

        double seconds = reader.GetDouble();
        return new EstimationTime(seconds); // Convert numeric value to EstimationTime
    }
}