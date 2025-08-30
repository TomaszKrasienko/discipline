using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discipline.centre.shared.infrastructure.Messaging.Outbox.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "outbox");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "outbox",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    JsonContent = table.Column<string>(type: "text", nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.MessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "outbox");
        }
    }
}
