using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationException(string message, ExceptionTypes exceptionType)
    : JiraReportsClientException(message, exceptionType);

public class ClientConfigurationMissingFileException(string fileName) 
    : ClientConfigurationException($"The file '{fileName}' doesn't exist.", ExceptionTypes.ConfigurationMissingFile);
    
    
public class ClientConfigurationMissingJsonSectionException(string fileName) 
    : ClientConfigurationException($"In the file '{fileName}', 'Jira' section doesn't exist.", ExceptionTypes.ConfigurationMissingJsonJiraSection)
{
    public string FileName { get; } = fileName;
}

public class ClientConfigurationMissingPropertiesException(string fileName, params string[] missingProperties) 
    : ClientConfigurationException($"One or more properties ({String.Join(", ", missingProperties)}) not found or have values in file '{fileName}'.", ExceptionTypes.ConfigurationMissingProperties)
{
    public string FileName { get; } = fileName;
    public string[] MissingProperties { get; } = missingProperties;
}

public class ClientConfigurationPropertyEmptyException<T>(string propertyName, string sectionPath) 
    : ClientConfigurationException($"Required configuration value '{propertyName}' of type '{typeof(T).Name}' in section '{sectionPath}' is missing or invalid.", ExceptionTypes.ConfigurationPropertyEmpty)
{
    public string PropertyName { get; } = propertyName;
    public string SectionPath { get; } = sectionPath;
}

public class ClientConfigurationPropertyNotNumberException(string fileName, string propertyName, string? propertyValue) 
    : ClientConfigurationException($"The property '{propertyName}' is not a number. Value: [{propertyValue}]", ExceptionTypes.ConfigurationPropertyNotNumber)
{
    public string FileName { get; } = fileName;
    public string PropertyName { get; } = propertyName;
    public string? PropertyValue { get; } = propertyValue;
}