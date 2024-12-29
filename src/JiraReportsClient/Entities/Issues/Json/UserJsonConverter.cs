using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Json;

public class UserJsonConverter : JsonConverter<User>
{
    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Name); 
    }

    public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (User)JsonSerializer.Deserialize(ref reader, typeof(User), options);
    }
}