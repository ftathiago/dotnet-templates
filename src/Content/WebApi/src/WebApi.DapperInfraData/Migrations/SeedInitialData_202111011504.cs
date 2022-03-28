using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.DapperInfraData.Migrations
{
    [ExcludeFromCodeCoverage]
    [Migration(202111011504, "Seed initial data")]
    public class SeedInitialData_202111011504 : Migration
    {
        public override void Up()
        {
            Insert
                .IntoTable("SampleTable")
                .Row(new { testProperty = "Test Property active" });
            Insert
                .IntoTable("SampleTable")
                .Row(new { testProperty = "Test property inactive", active = false });
        }

        public override void Down()
        {
            Delete
                .FromTable("SampleTable")
                .Row(new { testProperty = "Test Property active" });
            Delete
                .FromTable("SampleTable")
                .Row(new { testProperty = "Test property inactive" });
        }
    }
}
