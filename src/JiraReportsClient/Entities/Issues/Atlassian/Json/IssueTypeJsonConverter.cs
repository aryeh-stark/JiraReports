using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Atlassian.Json;

public class IssueTypeJsonConverter : JsonConverter<IssueType>
{
    public override void Write(Utf8JsonWriter writer, IssueType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Name); // Serialize only the Name property as a string
    }

    public override IssueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (IssueType)JsonSerializer.Deserialize(ref reader, typeof(IssueType), options);
    }
}