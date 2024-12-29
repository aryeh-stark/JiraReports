using System.Net;
using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Http;

public abstract class JiraHttpClientException(
    string message,
    string endpoint,
    HttpStatusCode statusCode,
    ExceptionTypes exceptionType)
    : JiraReportsClientException(message, exceptionType)
{
    #region Properties

    public string Endpoint { get; } = endpoint;

    public HttpStatusCode StatusCode { get; } = statusCode;

    #endregion

    public override string ToString()
    {
        var text = string.Join(Environment.NewLine, $"Endpoint: {Endpoint}", $"StatusCode: {StatusCode}",
            base.ToString());
        return text;
    }
}

public class JiraGetBoardsForProjectException(string projectKeyOrId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get boards for project '{projectKeyOrId}'.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetBoardsForProjectsError)
{
    public string ProjectKeyOrId { get; } = projectKeyOrId;

    public override string ToString()
    {
        var text = string.Join(Environment.NewLine, $"ProjectKeyOrId: {ProjectKeyOrId}", base.ToString());
        return text;
    }
}

public class JiraGetBoardsException(string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get boards",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetBoardsError)
{
    public override string ToString()
    {
        return base.ToString();
    }
}

public class JiraGetBoardByIdException(int boardId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get board by Id '{boardId}'",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetBoardByIdError)
{
    public int BoardId { get; } = boardId;

    public override string ToString()
    {
        var baseString = base.ToString();
        var boardIdString = $"Board Id: {BoardId}";

        return string.Join(Environment.NewLine, boardIdString, baseString);
    }
}

public class JiraGetBoardByIdDeserializationException(int boardId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get and deserialize board by Id '{boardId}'. Status Code: {statusCode}.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetBoardByIdDeserializationError)
{
    public int BoardId { get; } = boardId;

    public override string ToString()
    {
        var baseString = base.ToString();
        var boardIdString = $"Board Id: {BoardId}";

        return string.Join(Environment.NewLine, boardIdString, baseString);
    }
}

public class JiraGetSprintByIdException(int sprintId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException($"Failed to get sprint by id: {sprintId}.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintByIdError)
{
    public int SprintId { get; } = sprintId;

    public override string ToString()
    {
        var baseString = base.ToString();
        var sprintIdString = $"Sprint Id: {SprintId}";

        return string.Join(Environment.NewLine, sprintIdString, baseString);
    }
}

public class JiraGetSprintByIdDeserializationException(int sprintId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to deserialize sprint '{sprintId}'.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintDeserializationError)
{
    public int SprintId { get; } = sprintId;

    public override string ToString()
    {
        var baseString = base.ToString();
        var sprintIdString = $"Sprint Id: {SprintId}";

        return string.Join(Environment.NewLine, sprintIdString, baseString);
    }
}

public class JiraGetSprintsForBoardException(string message, int boardId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get sprints for board '{boardId}'. Message: {message}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintsForBoardError)
{
    public int BoardId { get; } = boardId;
    
    public override string ToString()
    {
        var baseString = base.ToString();
        var boardIdString = $"Board Id: {BoardId}";

        return string.Join(Environment.NewLine, boardIdString, baseString);
    }
}

public class JiraGetSprintsForBoardDeserializationException(int boardId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to deserialize response of get sprints for board '{boardId}'.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintsForBoardDeserializationError)
{
    public int BoardId { get; } = boardId;
    
    public override string ToString()
    {
        var baseString = base.ToString();
        var boardIdString = $"Board Id: {BoardId}";

        return string.Join(Environment.NewLine, boardIdString, baseString);
    }
}


public class JiraGetProjectsException(string message, string endpoint, HttpStatusCode statusCode, int startAt, int maxResults)
    : JiraHttpClientException(
        $"Failed to list projects (startAt: {startAt}, max results: {maxResults}). Message: {message}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetProjectsError)
{
    public int StartAt { get; } = startAt;
    public int MaxResults { get; } = maxResults;

    public override string ToString()
    {
        var baseString = base.ToString();
        var startAtString = $"Start At: {StartAt}";
        var maxResultsString = $"Max Results: {MaxResults}";

        return string.Join(Environment.NewLine, baseString, startAtString, maxResultsString);
    }
}

public class JiraGetProjectsDeserializationException(
    string content,
    string endpoint,
    HttpStatusCode statusCode,
    int startAt,
    int maxResults)
    : JiraHttpClientException(
        $"Failed to deserialize response of get projects. Content: {content}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetProjectsDeserializationError)
{
    public int StartAt { get; } = startAt;
    public int MaxResults { get; } = maxResults;
    
    public override string ToString()
    {
        var baseString = base.ToString();
        var startAtString = $"Start At: {StartAt}";
        var maxResultsString = $"Max Results: {MaxResults}";

        return string.Join(Environment.NewLine, baseString, startAtString, maxResultsString);
    }
}

public class JiraGetIssueByIdException(
    string responseMessage,
    string issueId,
    string endpoint,
    HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get issue by Id '{issueId}. Response Message: {responseMessage}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetIssueByIdError);

public class JiraGetIssuesByIdsException(
    string responseMessage,
    IEnumerable<string> issueIds,
    IEnumerable<string> batchIssueIds,
    string endpoint,
    HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get batch of issues by Ids '{string.Join(", ", batchIssueIds)}' from a total of {issueIds.Count()}. Response Message: {responseMessage}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetIssuesByIdsError)
{
    public IEnumerable<string> IssueIds { get; } = issueIds;
    public IEnumerable<string> BatchIssueIds { get; } = batchIssueIds;

    public override string ToString()
    {
        var baseString = base.ToString();
        var issueIdsString = $"Issue Ids: {string.Join(", ", IssueIds)}";
        var batchIssueIdsString = $"Batch Issue Ids: {string.Join(", ", BatchIssueIds)}";

        return string.Join(Environment.NewLine, baseString, issueIdsString, batchIssueIdsString);
    }
}

public class JiraGetIssuesForSprintException(int sprintId, string endpoint, string responseMessage, HttpStatusCode statusCode)
    : JiraHttpClientException($"Failed to get issues for sprint '{sprintId}'. ResponseMessage: {responseMessage}", endpoint,
        statusCode, ExceptionTypes.HttpGetIssuesForSprintError)
{
    public int SprintId { get; init; } = sprintId;
}

public class JiraGetIssuesForSprintDeserializationException(int sprintId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException($"Failed to deserialize issues for sprint '{sprintId}'", endpoint, statusCode,
        ExceptionTypes.HttpGetIssuesForSprintDeserializationError)
{
    public int SprintId { get; init; } = sprintId;
}

public class JiraGetSprintReportForBoardAndSprintException(string response, int boardId, int sprintId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException($"Failed to get sprint report for board '{boardId}' and sprint '{sprintId}'. Response: {response}", endpoint,
        statusCode, ExceptionTypes.HttpGetSprintReportForBoardAndSprintError)
{
    public int BoardId { get; init; } = boardId;
    public int SprintId { get; init; } = sprintId;
}

public class JiraGetSprintReportForBoardAndSprintDeserializationException(int boardId, int sprintId, string endpoint, HttpStatusCode statusCode)
    : JiraHttpClientException($"Failed to deserialize sprint report for board '{boardId}' and sprint '{sprintId}'", endpoint, statusCode,
        ExceptionTypes.HttpGetSprintReportForBoardAndSprintDeserializationError)
{
    public int BoardId { get; init; } = boardId;
    public int SprintId { get; init; } = sprintId;
}

public class JiraGetSprintBurndownForBoardAndSprintException(
    string response,
    int boardId,
    int sprintId,
    string endpoint, 
    HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to get sprint burndown for board '{boardId}' and sprint '{sprintId}'. Response: {response}",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintBurndownForBoardAndSprintError)
{
    public int BoardId { get; } = boardId;
    public int SprintId { get; } = sprintId;
}

public class JiraGetSprintBurndownForBoardAndSprintDeserializationException(
    int boardId,
    int sprintId,
    string endpoint, 
    HttpStatusCode statusCode)
    : JiraHttpClientException(
        $"Failed to deserialize sprint burndown for board '{boardId}' and sprint '{sprintId}'.",
        endpoint,
        statusCode,
        ExceptionTypes.HttpGetSprintBurndownForBoardAndSprintDeserializationError)
{
    public int BoardId { get; } = boardId;
    public int SprintId { get; } = sprintId;
}

public class JiraNotScrumBoardException(int boardId) : JiraReportsClientException(
    $"The board id '`{boardId}' is not SCRUM", ExceptionTypes.NotScrumBoardError)
{
    public int BoardId { get; init; } = boardId;
}