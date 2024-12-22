using System.Net;
using JiraReportsClient.Exceptions;

namespace JiraReportsClient.Http;

public abstract class JiraHttpClientException(string endpoint, HttpStatusCode statusCode) : JiraReportsClientException
{
    #region Properties

    public string Endpoint { get; } = endpoint;

    public HttpStatusCode StatusCode { get; } = statusCode;

    #endregion
}

public class JiraListBoardsForProjectException(string projectKeyOrId, string endpoint, HttpStatusCode statusCode)
    : JiraRep(
        $"Failed to get boards for project '{projectKeyOrId}'.",
        endpoint,
        statusCode,
        ExceptionTypes.GetBoardsForProjectError)
{
    public string ProjectKeyOrId { get; } = projectKeyOrId;

    public override string ToString()
    {
        var baseString = base.ToString();
        var projectKeyOrIdString = $"Project Key or Id: {ProjectKeyOrId}";

        return string.Join(Environment.NewLine, new[] { baseString, projectKeyOrIdString }
            .Where(s => !string.IsNullOrEmpty(s)));
    }
}