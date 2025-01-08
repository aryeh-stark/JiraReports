namespace JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

public record SheetTab
 {
     public string Name { get; set; } = string.Empty;
     public List<List<SheetCell>> Rows { get; set; } = [];
     public int RowCount => Rows.Count;
     public int ColumnCount => Rows.FirstOrDefault()?.Count ?? 0;
 }