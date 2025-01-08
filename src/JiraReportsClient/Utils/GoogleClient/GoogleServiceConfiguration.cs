using Microsoft.Extensions.Configuration;

namespace JiraReportsClient.Utils.GoogleClient;

public class GoogleServiceConfiguration(
    string credentialsJsonFile,
    string driveId,
    string serviceAccountEmail,
    string appName = GoogleServiceConfiguration.DefaultAppName,
    string googleDocsUrlPrefix = GoogleServiceConfiguration.DefaultGoogleDocsUrlPrefix,
    string driveSheetsSubFolder = GoogleServiceConfiguration.DefaultDriveSheetsSubFolder)
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
}