using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlannedTaskViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW public."PlannedTaskView"
                AS
                SELECT 
                    "Id"
                    , "ActivityRuleId"
                    , "AccountId"
                    , "PlannedFor"
                    , "CreatedAt"
                    , "PlannedEnable"
                    , "ActivityCreated"
                FROM public."PlannedTasks";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW public."PlannedTaskView";
                """);
        }
    }
}
