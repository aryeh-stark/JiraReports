using Google.Apis.Sheets.v4.Data;

namespace JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

public record SheetFormatting
{
    public static SheetFormatting BoldFont = new SheetFormatting { IsBold = true };
    public bool IsBold { get; init; }
    public Color? BackgroundColor { get; init; }
    
    public static implicit operator SheetFormatting(Color color) => new SheetFormatting { BackgroundColor = color };
}


