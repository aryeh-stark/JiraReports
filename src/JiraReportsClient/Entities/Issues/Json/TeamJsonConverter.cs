using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Json;

public class TeamJsonConverter : JsonConverter<Team>
{
    public override void Write(Utf8JsonWriter writer, Team value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Name); 
    }

    public override Team Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (Team)JsonSerializer.Deserialize(ref reader, typeof(Team), options);
    }
}