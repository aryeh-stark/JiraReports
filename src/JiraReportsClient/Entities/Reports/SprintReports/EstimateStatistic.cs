using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.SprintReports;

public class EstimateStatistic
{
    [JsonPropertyName("statFieldId")]
    public string StatFieldId { get; set; }

    [JsonPropertyName("statFieldValue")]
    public StatFieldValue StatFieldValue { get; set; }
}