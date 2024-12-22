namespace JiraReportsClient.Exceptions;

public class JiraReportsClientException : Exception
{
    public ExceptionTypes ExceptionType { get; }
    
    public JiraReportsClientException(string message, ExceptionTypes exceptionType) : base(message)
    {
        ExceptionType = exceptionType;
    }
    
    public JiraReportsClientException(string message, Exception innerException, ExceptionTypes exceptionType) : base(message, innerException)
    {
        ExceptionType = exceptionType;
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, $"[Exception Type: {ExceptionType}]", $"[Message: {Message}]");
    }
}