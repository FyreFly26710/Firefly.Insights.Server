using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Ai.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insights");

            migrationBuilder.CreateTable(
                name: "AiChatConversations",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AiModelId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiChatConversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AiProviders",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ApiKey = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPayloads",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPayloads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AiChatMessages",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AiChatConversationId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AiModelId = table.Column<long>(type: "bigint", nullable: true),
                    InputTokens = table.Column<int>(type: "integer", nullable: true),
                    OutputTokens = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiChatMessages_AiChatConversations_AiChatConversationId",
                        column: x => x.AiChatConversationId,
                        principalSchema: "insights",
                        principalTable: "AiChatConversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AiModels",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AiProviderId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Avatar = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Model = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ModelId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    InputPrice = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    OutputPrice = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiModels_AiProviders_AiProviderId",
                        column: x => x.AiProviderId,
                        principalSchema: "insights",
                        principalTable: "AiProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobLogs",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    JobType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    AiModelId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLogs_AiModels_AiModelId",
                        column: x => x.AiModelId,
                        principalSchema: "insights",
                        principalTable: "AiModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleJobs",
                schema: "insights",
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
                        principalSchema: "insights",
                        principalTable: "JobLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionLogs",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    JobLogId = table.Column<long>(type: "bigint", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutionPayloadId = table.Column<long>(type: "bigint", nullable: false),
                    InputTokens = table.Column<int>(type: "integer", nullable: false),
                    OutputTokens = table.Column<int>(type: "integer", nullable: false),
                    ReasoningTokens = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionLogs_ExecutionPayloads_ExecutionPayloadId",
                        column: x => x.ExecutionPayloadId,
                        principalSchema: "insights",
                        principalTable: "ExecutionPayloads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionLogs_JobLogs_JobLogId",
                        column: x => x.JobLogId,
                        principalSchema: "insights",
                        principalTable: "JobLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobFollowUps",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ParentJobLogId = table.Column<long>(type: "bigint", nullable: false),
                    JobLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobFollowUps_JobLogs_JobLogId",
                        column: x => x.JobLogId,
                        principalSchema: "insights",
                        principalTable: "JobLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiChatMessages_AiChatConversationId",
                schema: "insights",
                table: "AiChatMessages",
                column: "AiChatConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_AiModels_AiProviderId",
                schema: "insights",
                table: "AiModels",
                column: "AiProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_AiModels_ModelId",
                schema: "insights",
                table: "AiModels",
                column: "ModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleJobs_JobLogId",
                schema: "insights",
                table: "ArticleJobs",
                column: "JobLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLogs_ExecutionPayloadId",
                schema: "insights",
                table: "ExecutionLogs",
                column: "ExecutionPayloadId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLogs_JobLogId",
                schema: "insights",
                table: "ExecutionLogs",
                column: "JobLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobFollowUps_JobLogId",
                schema: "insights",
                table: "JobFollowUps",
                column: "JobLogId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_AiModelId",
                schema: "insights",
                table: "JobLogs",
                column: "AiModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiChatMessages",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "ArticleJobs",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "ExecutionLogs",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "JobFollowUps",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "AiChatConversations",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "ExecutionPayloads",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "JobLogs",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "AiModels",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "AiProviders",
                schema: "insights");
        }
    }
}
