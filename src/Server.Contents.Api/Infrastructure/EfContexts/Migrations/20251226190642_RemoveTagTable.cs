using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Contents.Api.Infrastructure.EfContexts.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTags_Tags_TagId",
                schema: "contents",
                table: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "contents");

            migrationBuilder.DropIndex(
                name: "IX_ArticleTags_TagId",
                schema: "contents",
                table: "ArticleTags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "contents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                schema: "contents",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTags_Tags_TagId",
                schema: "contents",
                table: "ArticleTags",
                column: "TagId",
                principalSchema: "contents",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
