using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Atlassian.Json;

public class IssueFieldsConverterFactory : JsonConverterFactory
{
    private readonly Dictionary<string, string> _mapping;

    /// <summary>
    /// Pass a dictionary that maps "customfield_xxx" -> "YourPropertyName"
    /// Example: { "customfield_10001": "Team", "customfield_10104": "StoryPoints" }
    /// </summary>
    public IssueFieldsConverterFactory(Dictionary<string, string> mapping)
    {
        _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
    }

    /// <summary>
    /// Tells the serializer we only handle the 'IssueFields' type.
    /// </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(IssueFields);
    }

    /// <summary>
    /// Creates the actual converter for 'IssueFields'.
    /// </summary>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new IssueFieldsConverter(_mapping);
    }

    /// <summary>
    /// Inner converter that does the read/write logic using the provided dictionary.
    /// </summary>
    private class IssueFieldsConverter : JsonConverter<IssueFields>
    {
        private readonly Dictionary<string, string> _mapping;

        public IssueFieldsConverter(Dictionary<string, string> mapping)
        {
            _mapping = mapping;
        }

        public override IssueFields Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            
            if (root.ValueKind != JsonValueKind.Object)
            {
                throw new JsonException($"Expected an object for IssueFields, but got {root.ValueKind}.");
            }

            // Create the instance
            var issueFields = new IssueFields();

            // Iterate over the JSON object
            foreach (var jsonProp in root.EnumerateObject())
            {
                var jsonPropName = jsonProp.Name; 
                var jsonPropValue = jsonProp.Value;

                // If "customfield_10020" -> "RelatedSprints", for example
                if (_mapping.TryGetValue(jsonPropName, out var targetPropertyName))
                {
                    var propInfo = typeof(IssueFields).GetProperty(targetPropertyName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (propInfo != null)
                    {
                        var deserializedValue = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), propInfo.PropertyType, options);
                        propInfo.SetValue(issueFields, deserializedValue);
                        continue;
                    }
                }

                // Fallback: If property name matches an existing property on IssueFields
                var fallbackProp = typeof(IssueFields).GetProperty(jsonPropName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (fallbackProp != null)
                {
                    if (fallbackProp.PropertyType.Equals(typeof(DateTime?)))
                    {
                        var fallbackDateTimeValue = jsonPropValue.GetString();
                        if (DateTime.TryParse(fallbackDateTimeValue, out var dateTime))
                        {
                            fallbackProp.SetValue(issueFields, dateTime);
                        }
                    }
                    else if (fallbackProp.PropertyType.Equals(typeof(DateTime)))
                    {
                        var fallbackDateTimeValue = jsonPropValue.GetString();
                        if (DateTime.TryParse(fallbackDateTimeValue, out var dateTime))
                        {
                            fallbackProp.SetValue(issueFields, dateTime);
                        }
                    }
                    else
                    {
                        var fallbackValue = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), fallbackProp.PropertyType, options);
                        fallbackProp.SetValue(issueFields, fallbackValue);
                    }
                }
            }

            return issueFields;
        }

        public override void Write(Utf8JsonWriter writer, IssueFields value, JsonSerializerOptions options)
        {
            // If you only need to deserialize, you can throw here or skip
            // This example also handles serialization, reversing the mapping.
            writer.WriteStartObject();

            foreach (var propInfo in typeof(IssueFields).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propValue = propInfo.GetValue(value);
                var csharpPropName = propInfo.Name;

                // If "Team" -> "customfield_10001" in the dictionary
                var customField = _mapping
                    .FirstOrDefault(kv => kv.Value.Equals(csharpPropName, StringComparison.OrdinalIgnoreCase));
                
                var finalJsonName = !string.IsNullOrEmpty(customField.Key)
                    ? customField.Key     // e.g. "customfield_10001"
                    : csharpPropName;     // fallback to property name

                writer.WritePropertyName(finalJsonName);
                JsonSerializer.Serialize(writer, propValue, options);
            }

            writer.WriteEndObject();
        }
    }
}