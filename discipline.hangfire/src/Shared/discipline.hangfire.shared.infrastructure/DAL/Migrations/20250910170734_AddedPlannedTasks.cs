using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlannedTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedTasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ActivityRuleId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    AccountId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    PlannedFor = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PlannedEnable = table.Column<bool>(type: "boolean", nullable: false),
                    ActivityCreated = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "PlannedTasks");
        }
    }
}
