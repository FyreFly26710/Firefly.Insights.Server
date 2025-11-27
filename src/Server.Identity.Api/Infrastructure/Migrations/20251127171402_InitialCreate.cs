using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Identity.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAccount = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UserPassword = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    UserName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    UserAvatar = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UserProfile = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UserRole = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "user"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "Auth");
        }
    }
}
