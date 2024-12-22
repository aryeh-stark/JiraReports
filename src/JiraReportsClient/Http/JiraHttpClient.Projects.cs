using System.Text.Json;
using JiraReportsClient.Entities.Projects;
using JiraReportsClient.Http.EndpointFluentBuilder;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    public async Task<List<Project>> GetProjectsAsync()
    {
        var projectCollection = new List<Project>();
        var isLastPage = false;
        var endpointBuilder = _endpointBuilder
            .Projects()
            .WithPagination(0, MaxResults);
        do
        {
            var endpoint = endpointBuilder.BuildNextPage();
            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithCallStep(CallSteps.BeforeCall)
                .WithRange(endpointBuilder)
                .Debug("");

            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .WithErrorResponse(message)
                    .Error("");
                throw new JiraGetProjectsException(message, endpoint, response.StatusCode, endpointBuilder.StartAt,
                    endpointBuilder.MaxResults);
            }

            var content = await response.Content.ReadAsStringAsync();
            var projectsResponse = JsonSerializer.Deserialize<ProjectsResponse>(content, _jsonOptions);
            if (projectsResponse == null || !projectsResponse.HasValues())
                throw new JiraGetProjectsDeserializationException(content, endpoint, response.StatusCode,
                    endpointBuilder.StartAt, endpointBuilder.MaxResults);

            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithStatusCode(response.StatusCode)
                .WithCallStep(CallSteps.AfterCall)
                .WithRange(endpointBuilder)
                .Debug(
                    $"Is Last Page: {projectsResponse.IsLast}. Count: {projectsResponse.Values.Count} Content Length: {content.Length}");

            projectCollection.AddRange(projectsResponse.Values);
            isLastPage = projectsResponse.IsLast;
        } while (!isLastPage);

        return projectCollection;
    }
}