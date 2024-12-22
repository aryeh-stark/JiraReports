using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Configurations;

public class ClientConfigurationException(string message, ExceptionTypes exceptionType)
    : JiraReportsClientException(message, exceptionType);
