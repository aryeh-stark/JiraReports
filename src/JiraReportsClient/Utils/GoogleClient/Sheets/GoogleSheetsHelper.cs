using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using JiraReportsClient.Utils.GoogleClient.Drive;
using JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

namespace JiraReportsClient.Utils.GoogleClient.Sheets;

public class GoogleSheetsHelper
{
    private readonly GoogleServiceConfiguration _config;
    private readonly GoogleDriveHelper _driveHelper;
    private SheetsService Service { get; }

    public GoogleSheetsHelper(GoogleServiceConfiguration configuration)
    {
        _config = configuration;
        Service = GetSheetsService(configuration.GetCredentialsPath(), configuration.AppName);
        _driveHelper = new GoogleDriveHelper(configuration);
    }

    private static SheetsService GetSheetsService(string credentialsPath, string appName)
    {
        var credential = GoogleCredential
            .FromFile(credentialsPath)
            .CreateScoped(SheetsService.Scope.Spreadsheets);

        return new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = appName
        });
    }
    
    public string? DeleteSpreadsheet(string spreadsheetId) => _driveHelper.DeleteFile(spreadsheetId);

    public SheetReference CreateSpreadsheet(SheetData sheetData)
    {
        var sheets = sheetData.Tabs.Select(tab => new Sheet
        {
            Properties = new SheetProperties
            {
                Title = tab.Name,
                GridProperties = new GridProperties
                {
                    RowCount = tab.RowCount,
                    ColumnCount = tab.ColumnCount
                }
            }
        }).ToList();

        var newSpreadsheet = Service.Spreadsheets.Create(new Spreadsheet
        {
            Properties = new SpreadsheetProperties { Title = sheetData.Title },
            Sheets = sheets
        }).Execute();

        var file = new Google.Apis.Drive.v3.Data.File { Name = sheetData.Title };
        var updateRequest = _driveHelper.Service.Files.Update(file, newSpreadsheet.SpreadsheetId);
        updateRequest.SupportsAllDrives = true;
        updateRequest.AddParents = _driveHelper.WorkingFolderId ?? _config.DriveId;
        updateRequest.RemoveParents = "root";
        updateRequest.Execute();
        
        // Share with organization
        _driveHelper.ShareFileWithDomain(newSpreadsheet.SpreadsheetId, 
            domain: "Evinced.com");

        for (int tabIndex = 0; tabIndex < sheetData.Tabs.Count; tabIndex++)
        {
            var tab = sheetData.Tabs[tabIndex];
            var sheetId = newSpreadsheet.Sheets[tabIndex].Properties.SheetId;
            var formatRequests = CreateFormatRequests(tab.Rows, sheetId);

            if (formatRequests.Count > 0)
            {
                Service.Spreadsheets.BatchUpdate(
                    new BatchUpdateSpreadsheetRequest { Requests = formatRequests },
                    newSpreadsheet.SpreadsheetId
                ).Execute();
            }

            UpdateSheetValues(newSpreadsheet.SpreadsheetId, tab);
        }

        return new SheetReference(
            newSpreadsheet.SpreadsheetId,
            newSpreadsheet.SpreadsheetUrl,
            sheetData.Title
        );
    }

    private static string ExtractDomainFromEmail(string email)
    {
        var parts = email.Split('@');
        return parts.Length == 2 ? parts[1] : throw new ArgumentException("Invalid email format");
    }

    private List<Request> CreateFormatRequests(List<List<SheetCell>> rows, int? sheetId)
    {
        var formatRequests = new List<Request>();

        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            for (int colIndex = 0; colIndex < rows[rowIndex].Count; colIndex++)
            {
                var cell = rows[rowIndex][colIndex];
                if (cell.Formatting != null)
                {
                    formatRequests.Add(new Request
                    {
                        RepeatCell = new RepeatCellRequest
                        {
                            Range = new GridRange
                            {
                                SheetId = sheetId,
                                StartRowIndex = rowIndex,
                                EndRowIndex = rowIndex + 1,
                                StartColumnIndex = colIndex,
                                EndColumnIndex = colIndex + 1
                            },
                            Cell = new CellData
                            {
                                UserEnteredFormat = new CellFormat
                                {
                                    TextFormat = new TextFormat { Bold = cell.Formatting.IsBold },
                                    BackgroundColor = cell.Formatting.BackgroundColor
                                }
                            },
                            Fields = "userEnteredFormat(textFormat,backgroundColor)"
                        }
                    });
                }
            }
        }

        return formatRequests;
    }

    private void UpdateSheetValues(string spreadsheetId, SheetTab tab)
    {
        var values = new ValueRange
        {
            Values = (IList<IList<object>>)tab.Rows
                .Select(row => (IList<object>)row.Select(cell => (object)cell.Value).ToList())
                .ToList()
        };

        var updateRequest = Service.Spreadsheets.Values.Update(
            values,
            spreadsheetId,
            $"{tab.Name}!A1:Z{tab.RowCount}");
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        updateRequest.Execute();
    }

    public SheetReference AppendToSpreadsheet(string spreadsheetId, SheetData newData)
    {
        var existingSheet = Service.Spreadsheets.Get(spreadsheetId).Execute();
        var firstSheet = existingSheet.Sheets[0];
        var sheetTitle = firstSheet.Properties.Title;

        var headerResponse = Service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetTitle}!A1:Z1").Execute();
        var existingHeaders = headerResponse.Values[0].Select(h => h.ToString()).ToList();

        foreach (var tab in newData.Tabs)
        {
            var mappedRow = MapRowToExistingHeaders(tab.Rows, existingHeaders);
            AppendRow(spreadsheetId, sheetTitle, mappedRow);
        }

        return new SheetReference(
            spreadsheetId,
            existingSheet.SpreadsheetUrl,
            sheetTitle);
    }

    private List<object> MapRowToExistingHeaders(List<List<SheetCell>> rows, List<string> existingHeaders)
    {
        var mappedRow = new List<object>();
        var headerRow = rows[0];
        var dataRow = rows[1];

        foreach (var header in existingHeaders)
        {
            var headerIndex = headerRow.FindIndex(cell =>
                cell.Value.ToString().Equals(header, StringComparison.OrdinalIgnoreCase));
            mappedRow.Add(headerIndex != -1 ? dataRow[headerIndex].Value : "");
        }

        return mappedRow;
    }

    private void AppendRow(string spreadsheetId, string sheetTitle, List<object> row)
    {
        var valueRange = new ValueRange { Values = new List<IList<object>> { row } };

        var appendRequest = Service.Spreadsheets.Values.Append(
            valueRange,
            spreadsheetId,
            $"{sheetTitle}!A:Z"
        );
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
        appendRequest.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
        appendRequest.Execute();
    }
}