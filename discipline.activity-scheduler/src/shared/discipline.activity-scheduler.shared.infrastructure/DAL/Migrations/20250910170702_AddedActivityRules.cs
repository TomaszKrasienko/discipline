using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedActivityRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityRules",
                columns: table => new
                {
                    ActivityRuleId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    AccountId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Mode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                name: "ActivityRules");
        }
    }
}
