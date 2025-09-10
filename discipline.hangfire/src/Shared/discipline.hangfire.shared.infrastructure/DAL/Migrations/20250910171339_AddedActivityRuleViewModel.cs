using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedActivityRuleViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW public."ActivityRulesView"
                AS
                SELECT
                      "ActivityRuleId"
                    , "AccountId"
                    , "Title"
                    , "Mode"
                    , "SelectedDays"
                FROM public."ActivityRules";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW public."ActivityRulesView";
                """);
        }
    }
}
