using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.SprintReports;

public class StatFieldValue
{
    [JsonPropertyName("value")]
    public double? Value { get; set; }
}