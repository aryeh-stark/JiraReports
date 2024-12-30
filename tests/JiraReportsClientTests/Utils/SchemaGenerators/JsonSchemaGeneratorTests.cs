using System.Text.Json;
using FluentAssertions;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;
using JiraReportsClient.Utils.SchemaGenerators;

namespace JiraReportsClientTests.Utils.SchemaGenerators;

public class JsonSchemaGeneratorTests
{
    [Fact]
    public void GenerateSchema_SprintReportEnriched_Test()
    {
        var schema = JsonSchemaGenerator.GenerateSchema<SprintReportEnriched>();
        schema.Should().NotBeNull();
    }
    
    [Fact]
    public void GenerateSchema_SprintMetrics_Test()
    {
        var schema = JsonSchemaGenerator.GenerateSchema<SprintMetrics>();
        schema.Should().NotBeNull();
        
        
    }
}