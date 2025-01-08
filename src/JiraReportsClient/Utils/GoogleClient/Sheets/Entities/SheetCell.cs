namespace JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

public abstract class SheetCell(object value, SheetFormatting? sheetFormatting, SheetCellRoles role)
{
    public object Value { get; } = value;
    public SheetFormatting? Formatting { get; } = sheetFormatting;
    public SheetCellRoles Role { get; } = role;
}

public class DataCell(object value, SheetFormatting? sheetFormatting = null)
    : SheetCell(value, sheetFormatting, SheetCellRoles.Data);

public class HeaderCell(object value, SheetFormatting? sheetFormatting = null)
    : SheetCell(value, sheetFormatting, SheetCellRoles.Header);

public class TotalCell(object value, SheetFormatting? sheetFormatting = null)
    : SheetCell(value, sheetFormatting, SheetCellRoles.Total);
    
    
public enum SheetCellRoles
{
    Header,
    Data,
    Total
}