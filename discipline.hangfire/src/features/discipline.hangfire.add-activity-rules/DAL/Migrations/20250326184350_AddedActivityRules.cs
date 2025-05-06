using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.add_activity_rules.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedActivityRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "activity-rules");

            migrationBuilder.CreateTable(
                name: "ActivityRules",
                schema: "activity-rules",
                columns: table => new
                {
                    ActivityRuleId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Mode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SelectedDays = table.Column<int[]>(type: "integer[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRules", x => x.ActivityRuleId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRules",
                schema: "activity-rules");
        }
    }
}
