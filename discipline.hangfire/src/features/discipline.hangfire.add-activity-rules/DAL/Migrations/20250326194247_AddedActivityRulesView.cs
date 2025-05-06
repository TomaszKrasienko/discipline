using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.add_activity_rules.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedActivityRulesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW "activity-rules"."ActivityRulesView" 
                AS
                SELECT 
                	"ActivityRuleId"
                	, "UserId"
                	, "Mode"
                	, "SelectedDays"
                FROM "activity-rules"."ActivityRules"
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW activity-rules.ActivityRulesView");
        }
    }
}
