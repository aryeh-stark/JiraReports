using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationPropertyNotNumberException(string fileName, string propertyName, string? propertyValue) 
    : ClientConfigurationException($"The property '{propertyName}' is not a number. Value: [{propertyValue}]", ExceptionTypes.ConfigurationMissingProperties)
{
    public string FileName { get; } = fileName;
    public string PropertyName { get; } = propertyName;
    public string? PropertyValue { get; } = propertyValue;
}