using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationMissingFileException(string fileName) 
    : ClientConfigurationException($"The file '{fileName}' doesn't exist.", ExceptionTypes.ConfigurationMissingFile)
{
    
}