using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationMissingJsonSectionException(string fileName) 
    : ClientConfigurationException($"In the file '{fileName}', 'Jira' section doesn't exist.", ExceptionTypes.ConfigurationMissingJsonJiraSection)
{
    public string FileName { get; } = fileName;
}