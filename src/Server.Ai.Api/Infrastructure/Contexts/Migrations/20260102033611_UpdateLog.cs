using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Ai.Api.Infrastructure.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_AiModels_AiModelId",
                schema: "ai",
                table: "JobLogs");

            migrationBuilder.DropIndex(
                name: "IX_JobFollowUps_JobLogId",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "ActionType",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "Payload",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.AlterColumn<long>(
                name: "AiModelId",
                schema: "ai",
                table: "JobLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentJobLogId",
                schema: "ai",
                table: "JobFollowUps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_JobFollowUps_JobLogId",
                schema: "ai",
                table: "JobFollowUps",
                column: "JobLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_AiModels_AiModelId",
                schema: "ai",
                table: "JobLogs",
                column: "AiModelId",
                principalSchema: "ai",
                principalTable: "AiModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_AiModels_AiModelId",
                schema: "ai",
                table: "JobLogs");

            migrationBuilder.DropIndex(
                name: "IX_JobFollowUps_JobLogId",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.DropColumn(
                name: "ParentJobLogId",
                schema: "ai",
                table: "JobFollowUps");

            migrationBuilder.AlterColumn<long>(
                name: "AiModelId",
                schema: "ai",
                table: "JobLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                schema: "ai",
                table: "JobFollowUps",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                schema: "ai",
                table: "JobFollowUps",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "ai",
                table: "JobFollowUps",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "ai",
                table: "JobFollowUps",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                schema: "ai",
                table: "JobFollowUps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                schema: "ai",
                table: "JobFollowUps",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobFollowUps_JobLogId",
                schema: "ai",
                table: "JobFollowUps",
                column: "JobLogId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_AiModels_AiModelId",
                schema: "ai",
                table: "JobLogs",
                column: "AiModelId",
                principalSchema: "ai",
                principalTable: "AiModels",
                principalColumn: "Id");
        }
    }
}
