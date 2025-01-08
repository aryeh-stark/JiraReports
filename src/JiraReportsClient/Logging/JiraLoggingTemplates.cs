namespace JiraReportsClient.Logging;

public static class JiraLoggingTemplates
{
    public static readonly string DefaultConsoleTemplate = string.Join("#",
        "** Time: {Timestamp:HH:mm:ss} **",
        "Level: {Level:u3}", 
        "Message: {Message:lj}",
        "- Level: {Level}",
        "- Project: {ProjectKey}",
        "- Board: {BoardId}",
        "- Sprint: {SprintId}",
        "- Issue: {IssueKey}",
        "- Issues: {IssuesKeys}",
        "- Action: {Action}",
        "- CallStep: {CallStep}",
        "- StatusCode: {StatusCode}",
        "- RequestUrl: {RequestUrl}",
        "- ProjectId: {ProjectId}",
        "- Range: [{StartAt} to {MaxResults}]",
        "- Error Response: {ErrorResponse}",
        "- Exception: {Exception}{NewLine}").Replace("#", "{NewLine}");
    
}