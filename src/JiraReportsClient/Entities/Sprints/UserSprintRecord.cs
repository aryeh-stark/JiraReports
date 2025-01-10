using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Sprints;

[JsonConverter(typeof(UserSprintRecordKeyConverter))]
public struct UserSprintRecord(
    string username,
    int sprintId,
    string sprintName,
    string sprintSequenceId,
    int boardId,
    string boardName) : IEquatable<UserSprintRecord>
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = username;
    
    [JsonPropertyName("sprintd")] 
    public int SprintId { get; } = sprintId;
    
    [JsonPropertyName("sprintName")] 
    public string SprintName { get; } = sprintName;
    
    [JsonPropertyName("sprintSequenceId")] 
    public string SprintSequenceId { get; } = sprintSequenceId;
    
    [JsonPropertyName("boardId")] 
    public int BoardId { get; } = boardId;
    
    [JsonPropertyName("boardName")] 
    public string BoardName { get; } = boardName;

    public UserSprintRecord(string username, Sprint sprint) : this(username, sprint.Id, sprint.Name, sprint.SequenceId,
        sprint.Board.Id,
        sprint.Board.Name)
    {
    }

    public static bool operator ==(UserSprintRecord left, UserSprintRecord right) => left.Equals(right);

    public static bool operator !=(UserSprintRecord left, UserSprintRecord right) => !(left == right);

    public bool Equals(UserSprintRecord other)
    {
        return
            Compare(Username, other.Username) &&
            SprintId == other.SprintId &&
            Compare(SprintName, other.SprintName) &&
            Compare(SprintSequenceId, other.SprintSequenceId) &&
            BoardId == other.BoardId &&
            Compare(BoardName, other.BoardName);

        bool Compare(string left, string right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is UserSprintRecord other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Username, SprintId, SprintName, SprintSequenceId, BoardId, BoardName);
    }
}

public class UserSprintRecordKeyConverter : JsonConverter<UserSprintRecord>
{
    public override UserSprintRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new UserSprintRecord(
            parts[0],
            int.Parse(parts[1]),
            parts[2],
            parts[3],
            int.Parse(parts[4]),
            parts[5]
        );
    }

    public override void Write(Utf8JsonWriter writer, UserSprintRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.Username}_{value.SprintId}_{value.SprintName}_{value.SprintSequenceId}_{value.BoardId}_{value.BoardName}");
    }
    
    public override UserSprintRecord ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new UserSprintRecord(
            parts[0],
            int.Parse(parts[1]),
            parts[2],
            parts[3],
            int.Parse(parts[4]),
            parts[5]
        );
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, UserSprintRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.Username}_{value.SprintId}_{value.SprintName}_{value.SprintSequenceId}_{value.BoardId}_{value.BoardName}");
    }
}