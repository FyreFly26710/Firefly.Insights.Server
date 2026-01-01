using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Ai.Api.Infrastructure.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class AddCacheToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseJson",
                schema: "ai",
                table: "ExecutionPayloads",
                newName: "Response");

            migrationBuilder.RenameColumn(
                name: "RequestJson",
                schema: "ai",
                table: "ExecutionPayloads",
                newName: "Prompt");

            migrationBuilder.AddColumn<int>(
                name: "ReasoningTokens",
                schema: "ai",
                table: "ExecutionLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasoningTokens",
                schema: "ai",
                table: "ExecutionLogs");

            migrationBuilder.RenameColumn(
                name: "Response",
                schema: "ai",
                table: "ExecutionPayloads",
                newName: "ResponseJson");

            migrationBuilder.RenameColumn(
                name: "Prompt",
                schema: "ai",
                table: "ExecutionPayloads",
                newName: "RequestJson");
        }
    }
}
