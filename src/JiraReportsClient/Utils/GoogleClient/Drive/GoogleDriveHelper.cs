using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using V3 = Google.Apis.Drive.v3.Data;

namespace JiraReportsClient.Utils.GoogleClient.Drive;

public class GoogleDriveHelper
{
    public GoogleServiceConfiguration Config { get; }

    public DriveService Service { get; }

    public string? WorkingFolderId { get; }

    public GoogleDriveHelper(GoogleServiceConfiguration config)
    {
        Config = config;
        Service = GetDriveService(config.GetCredentialsPath(), config.AppName);
        WorkingFolderId = EnsureDirectoryPath(config.DriveSheetsSubFolder, config.DriveId);
    }

    private static DriveService GetDriveService(string credentialsPath, string appName)
    {
        var credential = GoogleCredential
            .FromFile(credentialsPath)
            .CreateScoped(DriveService.ScopeConstants.Drive);

        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = appName
        });
    }

    public async Task<V3.File> UploadFile(string filePath, string mimeType)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = Path.GetFileName(filePath),
            Parents = [WorkingFolderId]
        };

        await using var stream = new FileStream(filePath, FileMode.Open);
        var request = Service.Files.Create(fileMetadata, stream, mimeType);
        request.SupportsAllDrives = true;

        var upload = await request.UploadAsync();
        if (upload.Status != UploadStatus.Completed)
            throw new Exception($"Upload failed: {upload.Exception?.Message}");

        return request.ResponseBody;
    }

    public IList<V3.File> SearchFiles(string query, string driveId)
    {
        var fullQuery = $"'{WorkingFolderId}' in parents and {query}";
        var request = Service.Files.List();
        request.Q = fullQuery;
        request.SupportsAllDrives = true;
        request.IncludeItemsFromAllDrives = true;
        request.DriveId = driveId;
        request.Corpora = "drive";
        request.Fields = "files(id, name, mimeType)";

        var result = request.Execute();
        return result.Files;
    }

    public string? DeleteFile(string fileId)
    {
        try
        {
            var request = Service.Files.Delete(fileId);
            request.SupportsAllDrives = true;
            var response = request.Execute();
            return response;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"File with ID '{fileId}' not found", ex);
        }
        catch (Google.GoogleApiException ex)
        {
            throw new Exception($"Failed to delete file with ID '{fileId}'", ex);
        }
    }
    
    public Google.Apis.Drive.v3.Data.File CreateDirectory(string directoryName, string parentFolderId)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = directoryName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = [parentFolderId]
        };

        var request = Service.Files.Create(fileMetadata);
        request.SupportsAllDrives = true;
        return request.Execute();
    }

    public string EnsureDirectoryPath(string path, string parentFolderId)
    {
        var parts = path.Split('/', '\\').Where(p => !string.IsNullOrWhiteSpace(p));
        var currentParentId = parentFolderId;

        foreach (var part in parts)
        {
            var request = Service.Files.List();
            request.Q = $"name = '{part}' and mimeType = 'application/vnd.google-apps.folder' and '{currentParentId}' in parents";
            request.SupportsAllDrives = true;
            request.IncludeItemsFromAllDrives = true;
            request.DriveId = Config.DriveId;
            request.Corpora = "drive";
            request.Fields = "files(id, name, mimeType)";

            var folder = request.Execute().Files.FirstOrDefault();
            if (folder == null)
            {
                folder = CreateDirectory(part, currentParentId);
            }
            currentParentId = folder.Id;
        }

        return currentParentId;
    }
    
    public void ShareFileWithDomain(string fileId, string domain = "evinced.com", DrivePermissionRole role = DrivePermissionRole.Writer)
    {
        var permission = new Google.Apis.Drive.v3.Data.Permission
        {
            Type = "domain",
            Role = role.ToString().ToLower(),
            Domain = domain
        };

        var request = Service.Permissions.Create(permission, fileId);
        request.SupportsAllDrives = true;
        request.SendNotificationEmail = false;
        request.Execute();
    }
    
    public void ShareFile(string fileId, string emailAddress, DrivePermissionRole role = DrivePermissionRole.Writer)
    {
        var permission = new Google.Apis.Drive.v3.Data.Permission
        {
            Type = "user",
            Role = role.ToString().ToLower(),
            EmailAddress = emailAddress
        };

        var request = Service.Permissions.Create(permission, fileId);
        request.SupportsAllDrives = true;
        request.SendNotificationEmail = true;
        request.Execute();
    }
}