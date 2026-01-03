using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Ai.Api.Infrastructure.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                schema: "ai",
                table: "AiModels",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AiModels_Model",
                schema: "ai",
                table: "AiModels",
                column: "Model",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiModels_Model",
                schema: "ai",
                table: "AiModels");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                schema: "ai",
                table: "AiModels");
        }
    }
}
