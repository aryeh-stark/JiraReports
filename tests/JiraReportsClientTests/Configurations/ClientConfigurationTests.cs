using JiraReportsClient.Configurations;

namespace JiraReportsClientTests.Configurations;

public class ClientConfigurationTests
{
    [Fact]
    public void LoadFromAppSettingsTest()
    {
        var config = ClientConfiguration.LoadFromAppSettings();
        Assert.NotNull(config);
        Assert.NotEmpty(config.BaseUrl);
        Assert.NotEmpty(config.UserEmail);
        Assert.NotEmpty(config.ApiToken);
        Assert.True(config.MaxResults > 0);
        Assert.True(config.JqlChunkSize > 0);
    }
}