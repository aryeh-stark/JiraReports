using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Boards;

[DebuggerDisplay("[{BoardId}] {BoardName} - {Username}")]
[JsonConverter(typeof(BoardUserRecordKeyConverter))]
public readonly struct BoardUserRecord(int boardId, string boardName, string username) : IEquatable<BoardUserRecord>
{
    [JsonPropertyName("boardId")]
    public int BoardId { get; } = boardId;

    [JsonPropertyName("boardName")]
    public string BoardName { get; } = boardName;
    
    [JsonPropertyName("username")]
    public string Username { get; } = username;

    public BoardUserRecord(Board board, string username)
        : this(board.Id, board.Name, username)
    {
    }
    
    public BoardUserRecord(Sprint sprint, string username) 
        : this(sprint.Board, username)
    {
    }

    public static bool operator ==(BoardUserRecord left, BoardUserRecord right) => left.Equals(right);

    public static bool operator !=(BoardUserRecord left, BoardUserRecord right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is BoardUserRecord other)
        {
            return BoardId == other.BoardId &&
                   Compare(BoardName, other.BoardName);
                   Compare(Username, other.Username);
        }

        return false;

        bool Compare(string left, string right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BoardId, BoardName, Username);
    }

    public bool Equals(BoardUserRecord other)
    {
        return BoardId == other.BoardId && BoardName == other.BoardName && Username == other.Username;
    }
}

public class BoardUserRecordKeyConverter : JsonConverter<BoardUserRecord>
{
    public override BoardUserRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new BoardUserRecord(int.Parse(parts[0]), parts[1], parts[2]);
    }

    public override void Write(Utf8JsonWriter writer, BoardUserRecord value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.BoardId}_{value.BoardName}_{value.Username}");
    }

    public override BoardUserRecord ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string keyString = reader.GetString();
        string[] parts = keyString.Split('_');
        return new BoardUserRecord(int.Parse(parts[0]), parts[1], parts[2]);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, BoardUserRecord value, JsonSerializerOptions options)
    {
        writer.WritePropertyName($"{value.BoardId}_{value.BoardName}_{value.Username}"); 
    }
}