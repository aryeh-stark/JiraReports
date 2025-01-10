using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Sprints;

[JsonConverter(typeof(SprintRecordKeyConverter))]
public readonly struct SprintRecord(int sprintId, string sprintName, string sprintSequenceId, int boardId, string boardName)
{
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

    public SprintRecord(Sprint sprint) : this(sprint.Id, sprint.Name, sprint.SequenceId, sprint.Board.Id,
        sprint.Board.Name)
    {
    }

    public static bool operator ==(SprintRecord left, SprintRecord right) => left.Equals(right);

    public static bool operator !=(SprintRecord left, SprintRecord right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is SprintRecord other)
        {
            return SprintId == other.SprintId &&
                   Compare(SprintName, other.SprintName) &&
                   Compare(SprintSequenceId, other.SprintSequenceId) &&
                   BoardId == other.BoardId &&
                   Compare(BoardName, other.BoardName);
        }

        return false;

        bool Compare(string left, string right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SprintId, SprintName, SprintSequenceId, BoardId, BoardName);
    }
}

public class SprintRecordKeyConverter : JsonConverter<SprintRecord>
{
    public override SprintRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new SprintRecord(
            int.Parse(parts[0]), 
            parts[1], 
            parts[2], 
            int.Parse(parts[3]), 
            parts[4]
        );
    }

    public override void Write(Utf8JsonWriter writer, SprintRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.SprintId}_{value.SprintName}_{value.SprintSequenceId}_{value.BoardId}_{value.BoardName}");
    }
    
    public override SprintRecord ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Same logic as in Read()
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new SprintRecord(
            int.Parse(parts[0]),
            parts[1],
            parts[2],
            int.Parse(parts[3]),
            parts[4]
        );
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, SprintRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.SprintId}_{value.SprintName}_{value.SprintSequenceId}_{value.BoardId}_{value.BoardName}");
    }
}