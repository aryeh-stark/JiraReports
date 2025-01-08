using JiraReportsClient.Utils.GoogleClient;
using JiraReportsClient.Utils.GoogleClient.Drive;

namespace JiraReportsClientTests.Utils.GoogleClient.Drive;

public class GoogleDriveHelperTests : IDisposable
{
    private readonly GoogleDriveHelper _helper;
    private readonly string _testFilePath;
    private string _uploadedFileId;
    private GoogleServiceConfiguration _config;

    public GoogleDriveHelperTests()
    {
        _config = GoogleServiceConfiguration.LoadFromAppSettings();
        _helper = new GoogleDriveHelper(_config);
        _testFilePath = Path.GetTempFileName();
        File.WriteAllText(_testFilePath, "Test content");
    }

    [Fact]
    public async Task FullFileLifecycle_Test()
    {
        // Upload
        var file = await _helper.UploadFile(_testFilePath, "text/plain");
        _uploadedFileId = file.Id;
        Assert.NotNull(file.Id);

        // Search
        var files = _helper.SearchFiles($"name = '{Path.GetFileName(_testFilePath)}'", _config.DriveId);
        Assert.Single(files);
        Assert.Equal(_uploadedFileId, files.First().Id);

        // Delete
        _helper.DeleteFile(_uploadedFileId);
        
        // Verify deletion
        var deletedFiles = _helper.SearchFiles($"name = '{Path.GetFileName(_testFilePath)}'", _config.DriveId);
        Assert.Empty(deletedFiles);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}