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

    public GoogleSheetsHelper() : this(GoogleServiceConfiguration.LoadFromAppSettings())
    {
        
    }
    
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

    public SheetReference CreateSpreadsheet(SheetData sheetData, bool horizontalLayout = true)
    {
        var sheets = sheetData.Tabs.Select(tab => new Sheet
        {
            Properties = new SheetProperties
            {
                Title = tab.Name,
                GridProperties = new GridProperties
                {
                    RowCount = horizontalLayout ? tab.RowCount : tab.ColumnCount,
                    ColumnCount = horizontalLayout ? tab.ColumnCount : tab.RowCount
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
            var formatRequests = CreateFormatRequests(tab.Rows, sheetId, horizontalLayout);

            if (formatRequests.Count > 0)
            {
                Service.Spreadsheets.BatchUpdate(
                    new BatchUpdateSpreadsheetRequest { Requests = formatRequests },
                    newSpreadsheet.SpreadsheetId
                ).Execute();
            }

            UpdateSheetValues(newSpreadsheet.SpreadsheetId, tab, horizontalLayout);
        }

        return new SheetReference(
            newSpreadsheet.SpreadsheetId,
            newSpreadsheet.SpreadsheetUrl,
            sheetData.Title
        );
    }

    private List<Request> CreateFormatRequests(List<List<SheetCell>> rows, int? sheetId, bool horizontalLayout)
    {
        var formatRequests = new List<Request>();
        var formattedRows = horizontalLayout ? rows : TransposeData(rows);

        for (int rowIndex = 0; rowIndex < formattedRows.Count; rowIndex++)
        {
            for (int colIndex = 0; colIndex < formattedRows[rowIndex].Count; colIndex++)
            {
                var cell = formattedRows[rowIndex][colIndex];
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

    private void UpdateSheetValues(string spreadsheetId, SheetTab tab, bool horizontalLayout)
    {
        var data = horizontalLayout ? tab.Rows : TransposeData(tab.Rows);

        var values = new ValueRange
        {
            Values = (IList<IList<object>>)data
                .Select(row => (IList<object>)row.Select(cell => (object)cell.Value).ToList())
                .ToList()
        };

        var range = horizontalLayout
            ? $"{tab.Name}!A1:{GetColumnName(tab.ColumnCount)}{tab.RowCount}"
            : $"{tab.Name}!A1:{GetColumnName(tab.RowCount)}{tab.ColumnCount}";

        var updateRequest = Service.Spreadsheets.Values.Update(
            values,
            spreadsheetId,
            range);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        updateRequest.Execute();
    }

    private List<List<SheetCell>> TransposeData(List<List<SheetCell>> originalData)
    {
        var transposedData = new List<List<SheetCell>>();

        // Assuming the first row contains headers
        var numRows = originalData[0].Count;
        var numCols = originalData.Count;

        for (int i = 0; i < numRows; i++)
        {
            var newRow = new List<SheetCell>();
            for (int j = 0; j < numCols; j++)
            {
                newRow.Add(originalData[j][i]);
            }
            transposedData.Add(newRow);
        }

        return transposedData;
    }

    // Helper function to get column name from index (e.g., 1 -> A, 2 -> B, 26 -> Z, 27 -> AA)
    private static string GetColumnName(int columnIndex)
    {
        var dividend = columnIndex;
        var columnName = String.Empty;

        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }

    public SheetReference AppendToSpreadsheet(string spreadsheetId, SheetData newData, bool horizontalLayout = true)
    {
        var existingSheet = Service.Spreadsheets.Get(spreadsheetId).Execute();
        var firstSheet = existingSheet.Sheets[0];
        var sheetTitle = firstSheet.Properties.Title;

        // Get existing headers 
        var headerRange = horizontalLayout ? $"{sheetTitle}!A1:Z1" : $"{sheetTitle}!A1:A";
        var headerResponse = Service.Spreadsheets.Values.Get(spreadsheetId, headerRange).Execute();
        var existingHeaders = headerResponse.Values[0].Select(h => h.ToString()).ToList();

        foreach (var tab in newData.Tabs)
        {
            var mappedRow = MapRowToExistingHeaders(tab.Rows, existingHeaders, horizontalLayout);
            AppendRow(spreadsheetId, sheetTitle, mappedRow, horizontalLayout);
        }

        return new SheetReference(
            spreadsheetId,
            existingSheet.SpreadsheetUrl,
            sheetTitle);
    }

    private List<object> MapRowToExistingHeaders(List<List<SheetCell>> rows, List<string> existingHeaders, bool horizontalLayout)
    {
        if (horizontalLayout)
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
        else
        {
            return MapRowToExistingHeadersVertical(rows, existingHeaders);
        }
    }

    private List<object> MapRowToExistingHeadersVertical(List<List<SheetCell>> rows, List<string> existingHeaders)
    {
        var mappedRow = new List<object>();
        var headerColumn = rows.Select(row => row[0]).ToList(); // Get the first column (headers)
        var dataColumn = rows.Select(row => row[1]).ToList(); // Get the second column (values)

        foreach (var header in existingHeaders)
        {
            var headerIndex = headerColumn.FindIndex(cell =>
                cell.Value.ToString().Equals(header, StringComparison.OrdinalIgnoreCase));
            mappedRow.Add(headerIndex != -1 ? dataColumn[headerIndex].Value : "");
        }

        return mappedRow;
    }


    private void AppendRow(string spreadsheetId, string sheetTitle, List<object> row, bool horizontalLayout)
    {
        var valueRange = new ValueRange { Values = horizontalLayout ? new List<IList<object>> { row } : TransposeRow(row) };

        var appendRange = horizontalLayout ? $"{sheetTitle}!A:Z" : $"{sheetTitle}!A:A";

        var appendRequest = Service.Spreadsheets.Values.Append(
            valueRange,
            spreadsheetId,
            appendRange
        );
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
        appendRequest.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
        appendRequest.Execute();
    }

    private IList<IList<object>> TransposeRow(List<object> row)
    {
        // Transpose the row to append vertically
        var transposedRow = new List<IList<object>>();
        foreach (var item in row)
        {
            transposedRow.Add(new List<object> { item });
        }
        return transposedRow;
    }
}