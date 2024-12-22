using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationMissingPropertiesException(string fileName, params string[] missingProperties) 
    : ClientConfigurationException($"One or more properties ({String.Join(", ", missingProperties)}) not found or have values in file '{fileName}'.", ExceptionTypes.ConfigurationMissingProperties)
{
    public string FileName { get; } = fileName;
    public string[] MissingProperties { get; } = missingProperties;
}