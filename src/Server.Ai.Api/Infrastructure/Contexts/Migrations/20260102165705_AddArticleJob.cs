using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Ai.Api.Infrastructure.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleJobs",
                schema: "ai",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    JobLogId = table.Column<long>(type: "bigint", nullable: false),
                    ArticleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleJobs_JobLogs_JobLogId",
                        column: x => x.JobLogId,
                        principalSchema: "ai",
                        principalTable: "JobLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleJobs_JobLogId",
                schema: "ai",
                table: "ArticleJobs",
                column: "JobLogId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleJobs",
                schema: "ai");
        }
    }
}
