using System.Text.Json;
using NJsonSchema;

namespace JiraReportsClient.Utils.SchemaGenerators;

public static class JsonSchemaGenerator
{
    public static async Task<string> GenerateSchemaAsync<T>()
    {
        var schema = await Task.Run(JsonSchema.FromType<T>);
        return schema.ToJson();
    }
    
    public static string GenerateSchema<T>()
    {
        var schema = JsonSchema.FromType<T>();
        return schema.ToJson();
    }

    public static string GeneratePrettySchema<T>()
    {
        var schema = JsonSchema.FromType<T>();
        var options = new JsonSerializerOptions { WriteIndented = true };

        // Parse and re-serialize to get consistent formatting
        var jsonDocument = JsonDocument.Parse(schema.ToJson());
        return JsonSerializer.Serialize(jsonDocument, options);
    }
}