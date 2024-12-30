using FluentAssertions;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Utils.SchemaGenerators;

namespace JiraReportsClientTests.Utils.SchemaGenerators;

public class CSharpSchemaGeneratorTests
{
    [Fact]
    public void GenerateSchemaTest()
    {
        var schema = CSharpSchemaGenerator.GenerateSchema<SprintReportEnriched>();
        schema.Should().NotBeNullOrEmpty();
    }
    
    
}