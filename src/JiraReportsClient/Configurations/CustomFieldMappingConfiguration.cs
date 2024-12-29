namespace JiraReportsClient.Configurations;

public class CustomFieldMappingConfiguration
{
    /// <summary>
    /// Holds the default mappings for custom fields, applicable to all projects unless overridden.
    /// 
    /// E.g., "StoryPoints" -> "customfield_10104"
    /// </summary>
    public Dictionary<string, string> Mapping { get; set; } = new();
}