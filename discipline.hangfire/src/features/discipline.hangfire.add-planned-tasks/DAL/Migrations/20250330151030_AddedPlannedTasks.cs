using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.add_planned_tasks.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlannedTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "planned-tasks");

            migrationBuilder.CreateTable(
                name: "PlannedTasks",
                schema: "planned-tasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ActivityRuleId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    PlannedFor = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsPlannedEnable = table.Column<bool>(type: "boolean", nullable: false),
                    IsActivityCreated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedTasks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedTasks",
                schema: "planned-tasks");
        }
    }
}
