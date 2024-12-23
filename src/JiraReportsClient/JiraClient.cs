using JiraReportsClient.Http;

namespace JiraReportsClient;

public class JiraClient(JiraHttpClient client)
{
    private readonly JiraHttpClient _client = client;
    
    
}