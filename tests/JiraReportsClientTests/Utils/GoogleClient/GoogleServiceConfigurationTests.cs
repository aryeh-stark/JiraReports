using FluentAssertions;
using JiraReportsClient.Utils.GoogleClient;

namespace JiraReportsClientTests.Utils.GoogleClient;

public class GoogleServiceConfigurationTests
{
    [Fact]
    public void LoadFromAppSettingsTest()
    {
        var googleConfig = GoogleServiceConfiguration.LoadFromAppSettings();

        // Assert
        googleConfig.Should().NotBeNull();
        googleConfig.AppName.Should().NotBeNullOrEmpty();
        googleConfig.CredentialsJsonFile.Should().NotBeNullOrEmpty();
        googleConfig.DriveId.Should().NotBeNullOrEmpty();
        googleConfig.GoogleDocsUrlPrefix.Should().NotBeNullOrEmpty();
        googleConfig.ServiceAccountEmail.Should().NotBeNullOrEmpty();
        
        var credentialsPath = googleConfig.GetCredentialsPath();
        credentialsPath.Should().NotBeNullOrEmpty();
    }
}