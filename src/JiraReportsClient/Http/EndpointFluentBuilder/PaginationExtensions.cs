namespace JiraReportsClient.Http.EndpointFluentBuilder;

public static class PaginationExtensions
{
    public static string BuildNextPage<T>(this T builder) where T : JiraEndpointBuilderBase<T>
    {
        if (builder is JiraEndpointBuilderBase<T> baseBuilder)
        {
            if (builder.BuildCounter == 0)
                return baseBuilder.Build();
            
            return baseBuilder.WithPagination(builder.StartAt + builder.MaxResults, builder.MaxResults).Build();
        }
        throw new InvalidOperationException("Builder must inherit from JiraEndpointBuilderBase");
    }

    public static IEnumerable<string> BuildPages<T>(this T builder, int totalPages) where T : JiraEndpointBuilderBase<T>
    {
        if (!(builder is JiraEndpointBuilderBase<T> baseBuilder))
            throw new InvalidOperationException("Builder must inherit from JiraEndpointBuilderBase");

        JiraEndpointBuilderBase<T> currentBuilder = baseBuilder;
        for (int i = 0; i < totalPages; i++)
        {
            yield return currentBuilder.Build();
            if (i < totalPages - 1)
            {
                currentBuilder = currentBuilder.WithPagination(
                    currentBuilder.StartAt + currentBuilder.MaxResults, 
                    currentBuilder.MaxResults);
            }
        }
    }
}