using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.hangfire.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccountViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW public."AccountsView"
                AS
                SELECT "AccountId"
                FROM public."Accounts";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP VIEW public."AccountsView";
                """);
        }
    }
}
