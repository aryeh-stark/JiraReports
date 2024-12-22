using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationPropertyEmptyException<T>(string propertyName, string sectionPath) 
    : ClientConfigurationException($"Required configuration value '{propertyName}' of type '{typeof(T).Name}' in section '{sectionPath}' is missing or invalid.", ExceptionTypes.ConfigurationPropertyEmpty)
{
    public string PropertyName { get; } = propertyName;
    public string SectionPath { get; } = sectionPath;
}