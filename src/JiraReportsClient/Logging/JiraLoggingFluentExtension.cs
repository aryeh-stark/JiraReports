using System.Net;
using System.Runtime.CompilerServices;
using JiraReportsClient.Http.EndpointFluentBuilder;
using Serilog;
using Serilog.Events;

namespace JiraReportsClient.Logging;

public static class JiraLoggingFluentExtension
{
    public static void Debug(this ILogger logger, string message, params object[] propertyValues)
    {
        logger.LogWithLevel(LogEventLevel.Debug, message, propertyValues);
    }

    public static void Info(this ILogger logger, string message, params object[] propertyValues)
    {
        logger.LogWithLevel(LogEventLevel.Information, message, propertyValues);
    }

    public static void Warning(this ILogger logger, string message, params object[] propertyValues)
    {
        logger.LogWithLevel(LogEventLevel.Warning, message, propertyValues);
    }

    public static void Error(this ILogger logger, string message, params object[] propertyValues)
    {
        logger.LogWithLevel(LogEventLevel.Error, message, propertyValues);
    }

    public static void Fatal(this ILogger logger, string message, params object[] propertyValues)
    {
        logger.LogWithLevel(LogEventLevel.Fatal, message, propertyValues);
    }

    public static ILogger WithProperty<T>(this ILogger logger, string propertyName, T propertyValue)
    {
        return logger.ForContext(propertyName, propertyValue);
    }

    public static ILogger WithEndpoint(this ILogger logger, string requestUrl)
    {
        return logger.WithProperty("RequestUrl", requestUrl);
    }

    public static ILogger WithBoardId(this ILogger logger, int boardId)
    {
        return logger.WithProperty("BoardId", boardId);
    }

    public static ILogger WithSprintId(this ILogger logger, int sprintId)
    {
        return logger.WithProperty("SprintId", sprintId);
    }

    public static ILogger WithIssueKey(this ILogger logger, string issueKey)
    {
        return logger.WithProperty("IssueKey", issueKey);
    }

    public static ILogger WithIssueKeys(this ILogger logger, string[] issueKeys)
    {
        return logger.WithProperty("IssuesKeys", string.Join(", ", issueKeys));
    }

    public static ILogger WithProjectId(this ILogger logger, int projectId)
    {
        return logger.WithProperty("ProjectId", projectId);
    }

    public static ILogger WithProjectKey(this ILogger logger, string projectKey)
    {
        return logger.WithProperty("ProjectKey", projectKey);
    }

    public static ILogger WithAction(this ILogger logger, [CallerMemberName] string action = "")
    {
        return logger.WithProperty("Action", action);
    }

    public static ILogger WithStatusCode(this ILogger logger, HttpStatusCode statusCode)
    {
        return logger.WithProperty("StatusCode", statusCode.ToString());
    }

    public static ILogger WithCallStep(this ILogger logger, CallSteps callStep)
    {
        return logger.WithProperty("CallStep", callStep == CallSteps.BeforeCall ? "Before Call" : "After Call");
    }

    public static ILogger WithRange(this ILogger logger, int startAt, int maxResults)
    {
        return logger
            .WithProperty("StartAt", startAt)
            .WithProperty("MaxResults", maxResults);
    }

    public static ILogger WithRange(this ILogger logger, IPaginatedBuilder builder)
    {
        return logger
            .WithProperty("StartAt", builder.StartAt)
            .WithProperty("MaxResults", builder.StartAt + builder.MaxResults);
    }
    
    public static ILogger WithErrorResponse(this ILogger logger, string responseContent)
    {
        return logger.WithProperty("ErrorResponse", responseContent);
    }

    public static ILogger WithException(this ILogger logger, Exception exception)
    {
        return logger.ForContext("Exception", exception.ToString());
    }


    public static ILogger WithSprintStates(this ILogger logger, bool future = false, bool active = false,
        bool closed = false)
    {
        if (!future && !active && !closed)
        {
            return logger.WithProperty("Sprint States", "all");
        }

        var sprintStates = new List<string>
        {
            "future: " + (future ? "included" : "omitted"),
            "active: " + (active ? "included" : "omitted"),
            "closed: " + (closed ? "included" : "omitted")
        }.Where(x => !string.IsNullOrEmpty(x)).ToList();
        return logger.WithProperty("SprintStates", string.Join(", ", sprintStates));
    }

    #region Private Methods

    private static void LogWithLevel(this ILogger logger, LogEventLevel level, string message,
        params object[] propertyValues)
    {
        logger.ForContext("Level", level.ToString()).Write(level, message, propertyValues);
    }

    #endregion

    
}