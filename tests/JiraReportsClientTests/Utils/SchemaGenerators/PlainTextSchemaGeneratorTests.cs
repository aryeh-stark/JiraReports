using FluentAssertions;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Utils.SchemaGenerators;

namespace JiraReportsClientTests.Utils.SchemaGenerators;

public class PlainTextSchemaGeneratorTests
{
    [Fact]
    public void GenerateSchemaTest()
    {
        var schema = PlainTextSchemaGenerator.GenerateSchema<SprintReportEnriched>();
        schema.Should().NotBeNullOrEmpty();
    }
}