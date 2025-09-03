using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.activity_rules.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(
                """
                DROP VIEW "activity-rules"."ActivityRulesView"
                """);
                            
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "activity-rules",
                table: "ActivityRules",
                newName: "AccountId");
            
            migrationBuilder.Sql(
                """
                CREATE VIEW "activity-rules"."ActivityRulesView"
                AS
                SELECT
                    "ActivityRuleId",
                    "AccountId",
                    "Title",
                    "Mode",
                    "SelectedDays"
                FROM "activity-rules"."ActivityRules";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW "activity-rules"."ActivityRulesView"
                """);
            
            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "activity-rules",
                table: "ActivityRules",
                newName: "UserId");
            
            migrationBuilder.Sql(
                """
                CREATE VIEW "activity-rules"."ActivityRulesView"
                AS
                SELECT
                    "ActivityRuleId",
                    "UserId",
                    "Title",
                    "Mode",
                    "SelectedDays"
                FROM "activity-rules"."ActivityRules";
                """);
        }
    }
}
