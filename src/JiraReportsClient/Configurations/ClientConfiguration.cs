using Microsoft.Extensions.Configuration;
using JiraReportsClient.Configurations.Exceptions;

namespace JiraReportsClient.Configurations;

public class ClientConfiguration
{
    private const string JiraSectionName = "Jira";

    // Properties which must be present in the "Jira" section
    private static readonly IEnumerable<string> AppSettingsPropertyNames = new[]
    {
        "BaseURL",
        "UserEmail",
        "ApiToken",
        "MaxResults",
        "JqlChunkSize"
    };

    // Properties that must be numeric
    private static readonly IEnumerable<string> AppSettingsNumericPropertyNames = new[]
    {
        "MaxResults",
        "JqlChunkSize"
    };

    public string BaseUrl { get; }
    public string UserEmail { get; }
    public string ApiToken { get; }
    public int MaxResults { get; }
    public int JqlChunkSize { get; }
    public CustomFieldMappingConfiguration CustomFieldMapping { get; }

    public ClientConfiguration()
    {
        BaseUrl = string.Empty;
        UserEmail = string.Empty;
        ApiToken = string.Empty;
        MaxResults = 0;
        JqlChunkSize = 0;
        CustomFieldMapping = new CustomFieldMappingConfiguration();
    }

    public ClientConfiguration(
        string baseUrl,
        string userEmail,
        string apiToken,
        int maxResults,
        int jqlChunkSize,
        CustomFieldMappingConfiguration customFieldMapping)
    {
        ArgumentNullException.ThrowIfNull(baseUrl);
        ArgumentNullException.ThrowIfNull(userEmail);
        ArgumentNullException.ThrowIfNull(apiToken);
        ArgumentNullException.ThrowIfNull(customFieldMapping);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxResults);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(jqlChunkSize);

        BaseUrl = baseUrl;
        UserEmail = userEmail;
        ApiToken = apiToken;
        MaxResults = maxResults;
        JqlChunkSize = jqlChunkSize;
        CustomFieldMapping = customFieldMapping;
    }

    public override string ToString()
    {
        var content = string.Join("] [",
            $"BaseUrl: {BaseUrl}",
            $"UserEmail: {UserEmail}",
            $"ApiToken (Length): {ApiToken.Length}",
            $"MaxResults: {MaxResults}",
            $"JqlChunkSize: {JqlChunkSize}",
            $"CustomFieldMapping: {string.Join("; ", CustomFieldMapping.Mapping.Select(kv => $"{kv.Key} => {kv.Value}"))}"
        );

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
        // Get the "Jira" section
        var section = configuration.GetSection(JiraSectionName);
        if (section == null)
            throw new ClientConfigurationMissingJsonSectionException(fileName);

        // Check if required properties are missing
        var missingProperties = AppSettingsPropertyNames
            .Where(propName => section[propName] == null || string.IsNullOrEmpty(section[propName]))
            .ToArray();

        if (missingProperties.Length > 0)
        {
            throw new ClientConfigurationMissingPropertiesException(fileName, missingProperties);
        }

        // Fetch required values
        var baseUrl = section.GetRequiredValue("BaseURL");
        var userEmail = section.GetRequiredValue("UserEmail");
        var token = section.GetRequiredValue("ApiToken");

        // Verify numeric properties
        foreach (var numericPropertyName in AppSettingsNumericPropertyNames)
        {
            if (!int.TryParse(section[numericPropertyName], out _))
            {
                throw new ClientConfigurationPropertyNotNumberException(
                    fileName,
                    numericPropertyName,
                    section[numericPropertyName]
                );
            }
        }

        var maxResults = int.Parse(section["MaxResults"]!);
        var jqlChunkSize = int.Parse(section["JqlChunkSize"]!);

        var customFieldMappingSection = section.GetSection("CustomFieldMapping");
        var customFieldMapping = new CustomFieldMappingConfiguration();
        customFieldMappingSection.Bind(customFieldMapping);

        // Create final configuration object
        return new ClientConfiguration(
            baseUrl!,
            userEmail!,
            token!,
            maxResults,
            jqlChunkSize,
            customFieldMapping
        );
    }
}