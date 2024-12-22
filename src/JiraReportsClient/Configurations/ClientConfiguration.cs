using Microsoft.Extensions.Configuration;
using JiraReportsClient.Configurations.Exceptions;

namespace JiraReportsClient.Configurations;

public class ClientConfiguration
{
    private const string JiraSectionName = "Jira";

    private static readonly IEnumerable<string> AppSettingsPropertyNames =
    [
        "BaseURL",
        "UserEmail",
        "ApiToken",
        "MaxResults",
        "JqlChunkSize"
    ];

    private static readonly IEnumerable<string> AppSettingsNumericPropertyNames =
    [
        "MaxResults",
        "JqlChunkSize"
    ];

    public string BaseUrl { get; }
    public string UserEmail { get; }
    public string ApiToken { get; }
    public int MaxResults { get; }
    public int JqlChunkSize { get; }

    public ClientConfiguration()
    {
        BaseUrl = string.Empty;
        UserEmail = string.Empty;
        ApiToken = string.Empty;
        MaxResults = 0;
        JqlChunkSize = 0;
    }

    public ClientConfiguration(string baseUrl, string userEmail, string apiToken, int maxResults,
        int jqlChunkSize)
    {
        ArgumentNullException.ThrowIfNull(baseUrl);
        ArgumentNullException.ThrowIfNull(userEmail);
        ArgumentNullException.ThrowIfNull(apiToken);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxResults);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(jqlChunkSize);

        BaseUrl = baseUrl;
        UserEmail = userEmail;
        ApiToken = apiToken;
        MaxResults = maxResults;
        JqlChunkSize = jqlChunkSize;
    }

    public override string ToString()
    {
        var content = string.Join("] [",
            $"BaseUrl: {BaseUrl}",
            $"UserEmail: {UserEmail}",
            $"ApiToken (Length): {ApiToken.Length}",
            $"MaxResults: {MaxResults}",
            $"JqlChunkSize: {JqlChunkSize}");
        return $"[{content}]";
    }


    public static ClientConfiguration LoadFromAppSettings()
    {
        return LoadFromJsonFile("appsettings.json");
    }

    public static ClientConfiguration LoadFromJsonFile(string fileName)
    {
        if (!File.Exists(fileName))
            throw new ClientConfigurationMissingFileException(fileName);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(fileName, optional: false, reloadOnChange: false)
            .Build();
        return LoadFromConfiguration(fileName, configuration);
    }

    private static ClientConfiguration LoadFromConfiguration(string fileName, IConfigurationRoot configuration)
    {
        var section = configuration.GetSection(JiraSectionName);
        if (section == null)
            throw new ClientConfigurationMissingJsonSectionException(fileName);

        var missingProperties = AppSettingsPropertyNames
            .Where(x => section[x] == null || String.IsNullOrEmpty(x))
            .ToArray();
        if (missingProperties.Length > 0)
        {
            throw new ClientConfigurationMissingPropertiesException(fileName, missingProperties);
        }

        var baseUrl = section.GetRequiredValue("BaseURL");
        var userEmail = section.GetRequiredValue("UserEmail");
        var token = section.GetRequiredValue("ApiToken");

        AppSettingsNumericPropertyNames.ToList().ForEach(propertyName =>
        {
            if (!int.TryParse(section[propertyName], out _))
                throw new ClientConfigurationPropertyNotNumberException(fileName, propertyName, section[propertyName]);
        });

        var maxResults = int.Parse(section["MaxResults"]!);
        var jqlChunkSize = int.Parse(section["JqlChunkSize"]!);

        var clientConfiguration =
            new ClientConfiguration(baseUrl!, userEmail!, token!, maxResults, jqlChunkSize);
        return clientConfiguration;
    }
}