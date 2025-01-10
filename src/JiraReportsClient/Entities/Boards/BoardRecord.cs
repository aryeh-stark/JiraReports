using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Boards;

[DebuggerDisplay("[{BoardId}] {BoardName}")]
[JsonConverter(typeof(BoardRecordKeyConverter))]
public struct BoardRecord(int boardId, string boardName)
{
    [JsonPropertyName("boardId")]
    public int BoardId { get; } = boardId;

    [JsonPropertyName("boardName")]
    public string BoardName { get; } = boardName;

    public BoardRecord(Board board)
        : this(board.Id, board.Name)
    {
    }
    
    public BoardRecord(Sprint sprint) 
        : this(sprint.Board)
    {
    }

    public static bool operator ==(BoardRecord left, BoardRecord right) => left.Equals(right);

    public static bool operator !=(BoardRecord left, BoardRecord right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is BoardRecord other)
        {
            return BoardId == other.BoardId &&
                   Compare(BoardName, other.BoardName);
        }

        return false;

        bool Compare(string left, string right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BoardId, BoardName);
    }

    public static implicit operator BoardRecord(JiraBoard board)
    {
        return new BoardRecord(board);
    }
    
    public static implicit operator BoardRecord(Board board)
    {
        return new BoardRecord(board);
    }

}

public class BoardRecordKeyConverter : JsonConverter<BoardRecord>
{
    public override BoardRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new BoardRecord(int.Parse(parts[0]), parts[1]);
    }

    public override void Write(Utf8JsonWriter writer, BoardRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.BoardId}_{value.BoardName}");
    }

    public override BoardRecord ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new BoardRecord(int.Parse(parts[0]), parts[1]);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, BoardRecord value, JsonSerializerOptions options)
    {
        writer.WritePropertyName($"{value.BoardId}_{value.BoardName}"); 
    }
}