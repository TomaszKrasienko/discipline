using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.browse_planned.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlannedTaskViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "browse-planned");
            
            migrationBuilder.Sql(
                """
                CREATE VIEW "browse-planned"."PlannedTasksView"
                AS 
                SELECT 
                	  ar."ActivityRuleId" AS "ActivityRuleId" 
                	, pt."PlannedFor" AS "PlannedFor"
                	, pt."CreatedAt" AS "CreatedAt"
                	, pt."IsPlannedEnable" AS "IsPlannedEnabled"
                	, pt."IsActivityCreated" AS "IsActivityCreated"
                	, ar."UserId" AS "UserId"
                FROM "planned-tasks"."PlannedTasks" pt 
                INNER JOIN "activity-rules"."ActivityRules" ar ON pt."ActivityRuleId" = ar."ActivityRuleId"
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW "browse-planned"."PlannedTasksView"
                """);
        }
    }
}
