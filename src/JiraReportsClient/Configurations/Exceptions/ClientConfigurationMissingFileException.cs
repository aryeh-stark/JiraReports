using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations;

public class ClientConfigurationMissingFileException(string fileName) 
    : ClientConfigurationException($"The file '{fileName}' doesn't exist.", ExceptionTypes.ConfigurationMissingFile)
{
    
}