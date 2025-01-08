namespace JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

public record SheetData
{
    public string Title { get; init; }
    public List<SheetTab> Tabs { get; init; }
}