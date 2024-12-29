using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Atlassian.Json;

public class IssueFieldsConverter(Dictionary<string, string> customFieldMapping) : JsonConverter<IssueFields>
{
    private readonly Dictionary<string, string> _customFieldMapping = customFieldMapping;
    
    public override IssueFields Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the incoming JSON into a JsonDocument for inspection
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        // We expect an object here. Throw if it's not.
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException($"Expected an object for IssueFields but got {root.ValueKind}.");
        }

        // Create an instance of IssueFields
        var issueFields = new IssueFields();

        // Enumerate the JSON object
        foreach (var jsonProp in root.EnumerateObject())
        {
            var jsonPropName = jsonProp.Name;               // e.g. "customfield_10020"
            var jsonPropValue = jsonProp.Value;            // The value

            // If this property is in our custom field dictionary, rename it
            if (_customFieldMapping.TryGetValue(jsonPropName, out var targetPropertyName))
            {
                // Look for "Team", "StoryPoints", "RelatedSprints", etc.
                var propInfo = typeof(IssueFields).GetProperty(
                    targetPropertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
                );

                if (propInfo != null)
                {
                    // Convert the JSON value into the correct property type
                    object? deserializedValue = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), propInfo.PropertyType, options);
                    propInfo.SetValue(issueFields, deserializedValue);
                    continue;
                }
            }

            // Otherwise, see if the property name matches a real property on IssueFields (like "priority", "status")
            // so we don't lose the non-customfield properties.
            var fallbackProp = typeof(IssueFields).GetProperty(
                jsonPropName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
            );

            if (fallbackProp != null)
            {
                object? fallbackValue = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), fallbackProp.PropertyType, options);
                fallbackProp.SetValue(issueFields, fallbackValue);
            }
        }

        return issueFields;
    }

    public override void Write(Utf8JsonWriter writer, IssueFields value, JsonSerializerOptions options)
    {
        // If you don’t need to serialize IssueFields back to JSON, you can throw or write a partial implementation.
        // For completeness, here's an example that reverts property names back to "customfield_xxx".
        writer.WriteStartObject();

        // Reflect over each property in IssueFields
        foreach (var propInfo in typeof(IssueFields).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propValue = propInfo.GetValue(value);
            var propName = propInfo.Name;

            // Check if we have a reverse mapping: "Team" -> "customfield_10001"
            var customFieldKvp = _customFieldMapping.FirstOrDefault(kv => kv.Value.Equals(propName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(customFieldKvp.Key))
            {
                // Write it as e.g. "customfield_10001"
                writer.WritePropertyName(customFieldKvp.Key);
            }
            else
            {
                // fallback: write as the original property name
                writer.WritePropertyName(propName);
            }

            // Serialize the property value
            JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteEndObject();
    }
}