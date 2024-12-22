using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations.Exceptions;

public class ClientConfigurationException(string message, ExceptionTypes exceptionType)
    : JiraReportsClientException(message, exceptionType);
