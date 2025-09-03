using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.add_planned_tasks.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "planned-tasks",
                table: "PlannedTasks",
                newName: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "planned-tasks",
                table: "PlannedTasks",
                newName: "UserId");
        }
    }
}
