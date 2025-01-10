using System.Globalization;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;

namespace JiraReportsClient.Utils.GoogleClient;

public class GoogleServiceConfiguration(
    string credentialsJsonFile,
    string driveId,
    string serviceAccountEmail,
    string appName = GoogleServiceConfiguration.DefaultAppName,
    string googleDocsUrlPrefix = GoogleServiceConfiguration.DefaultGoogleDocsUrlPrefix,
    string driveSheetsSubFolder = GoogleServiceConfiguration.DefaultDriveSheetsSubFolder,
    string headerBackgroundColor = "#f1f1f1",
    string sprintBackgroundColor = "#f5f5f5")
{
    private const string DefaultGoogleDocsUrlPrefix = "https://docs.google.com/spreadsheets/d/";
    private const string DefaultAppName = "JiraReportsClient-GoogleDrive";
    private const string DefaultDriveSheetsSubFolder = "Reports/SprintMetrics";
    public string AppName { get; set; } = appName;
    public string CredentialsJsonFile { get; set; } = credentialsJsonFile;
    public string ServiceAccountEmail { get; set; } = serviceAccountEmail;
    public string GoogleDocsUrlPrefix { get; set; } = googleDocsUrlPrefix;
    public string DriveId { get; set; } = driveId;
    public string DriveSheetsSubFolder { get; set; } = driveSheetsSubFolder;
    public Color HeaderBackgroundColor { get; set; } = HexToColor(headerBackgroundColor);
    public Color SprintBackgroundColor { get; set; } = HexToColor(sprintBackgroundColor);


    public GoogleServiceConfiguration() : this(string.Empty, string.Empty, string.Empty)
    {
    }


    public string GetCredentialsPath()
    {
        if (string.IsNullOrWhiteSpace(CredentialsJsonFile) || !File.Exists(CredentialsJsonFile))
        {
            throw new FileNotFoundException($"Google API JSON File not found");
        }

        return CredentialsJsonFile;
    }


    public static GoogleServiceConfiguration LoadFromAppSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var googleSheetsConfig = new GoogleServiceConfiguration();
        configuration.GetSection("Google").Bind(googleSheetsConfig);
        return googleSheetsConfig;
    }
    
    public static Color HexToColor(string hexColor)
    {
        // Remove the # if present
        if (hexColor.StartsWith("#"))
        {
            hexColor = hexColor.Substring(1);
        }

        // Parse the hex string to an integer
        if (int.TryParse(hexColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb))
        {
            // Extract the red, green, and blue components
            var red = (byte)((rgb >> 16) & 0xFF);
            var green = (byte)((rgb >> 8) & 0xFF);
            var blue = (byte)(rgb & 0xFF);

            // Convert to float values between 0 and 1
            return new Color
            {
                Red = red / 255f,
                Green = green / 255f,
                Blue = blue / 255f
            };
        }
        else
        {
            throw new ArgumentException("Invalid hex color format.");
        }
    }
}