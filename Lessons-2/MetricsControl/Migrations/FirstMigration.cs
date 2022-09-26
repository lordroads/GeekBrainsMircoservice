using FluentMigrator;

namespace MetricsControl.Migrations;

[Migration(1)]
public class FirstMigration : Migration
{
    public override void Down()
    {
        Delete.Table("infoofagents");
    }

    public override void Up()
    {
        Create.Table("infoofagents")
        .WithColumn("Id").AsInt64().PrimaryKey().Identity()
        .WithColumn("Address").AsString()
        .WithColumn("Enable").AsBoolean();
    }
}
