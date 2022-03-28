using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.DapperInfraData.Migrations
{
    [ExcludeFromCodeCoverage]
    [Migration(202111011503)]
    public class InitializeDatabase_202111011503 : Migration
    {
        private const string SampleTable = "SampleTable";
        public override void Up()
        {
            Create.Table(SampleTable)
                .WithColumn("id")
                    .AsInt64()
                    .Identity()
                    .NotNullable()
                .WithColumn("testProperty")
                    .AsString(50)
                    .NotNullable()
                .WithColumn("active")
                    .AsBoolean()
                    .WithDefaultValue(true)
                    .NotNullable();
            Create
                .PrimaryKey("IDX_PK_SampleTable")
                .OnTable(SampleTable)
                .Column("id");
        }
        public override void Down() =>
            Delete.Table(SampleTable);
    }
}
