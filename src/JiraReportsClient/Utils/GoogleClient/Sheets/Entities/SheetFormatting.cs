using Google.Apis.Sheets.v4.Data;

namespace JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

public record SheetFormatting
{
    public bool IsBold { get; init; }
    public Color? BackgroundColor { get; init; }
}