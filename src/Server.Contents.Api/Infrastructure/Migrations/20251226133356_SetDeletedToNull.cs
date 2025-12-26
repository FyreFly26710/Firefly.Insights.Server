using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Contents.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetDeletedToNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "contents",
                table: "Topics",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "contents",
                table: "ArticleTags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "contents",
                table: "ArticleTags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "contents",
                table: "ArticleTags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "contents",
                table: "Articles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "contents",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "contents",
                table: "Articles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<long>(
                name: "TopicId",
                schema: "contents",
                table: "ArticleMetas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "contents",
                table: "ArticleTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "contents",
                table: "ArticleTags");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "contents",
                table: "ArticleTags");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "contents",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "contents",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "contents",
                table: "Articles");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "contents",
                table: "Topics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TopicId",
                schema: "contents",
                table: "ArticleMetas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
