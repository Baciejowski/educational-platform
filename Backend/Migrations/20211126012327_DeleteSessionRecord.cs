using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Backend.Migrations
{
    public partial class DeleteSessionRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnsweredQuestions_AnalysisResults_AnalysisResultID",
                table: "AnsweredQuestions");

            migrationBuilder.DropTable(
                name: "SessionRecords");

            migrationBuilder.DropTable(
                name: "AnalysisResults");

            migrationBuilder.DropTable(
                name: "GameplayData");

            migrationBuilder.DropIndex(
                name: "IX_AnsweredQuestions_AnalysisResultID",
                table: "AnsweredQuestions");

            migrationBuilder.DropColumn(
                name: "AnalysisResultID",
                table: "AnsweredQuestions");

            migrationBuilder.RenameColumn(
                name: "SessionRecordID",
                table: "Sessions",
                newName: "Money");

            migrationBuilder.AddColumn<double>(
                name: "DifficultyLevel",
                table: "Sessions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Sessions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameplayTime",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Light",
                table: "Sessions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "ScenarioEnded",
                table: "Sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Speed",
                table: "Sessions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Vision",
                table: "Sessions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "GameplayTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Light",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ScenarioEnded",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Vision",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "Money",
                table: "Sessions",
                newName: "SessionRecordID");

            migrationBuilder.AddColumn<int>(
                name: "AnalysisResultID",
                table: "AnsweredQuestions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnalysisResults",
                columns: table => new
                {
                    AnalysisResultID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DifficultyLevel = table.Column<double>(type: "double precision", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ScenarioEnded = table.Column<bool>(type: "boolean", nullable: false),
                    SessionRecordID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResults", x => x.AnalysisResultID);
                });

            migrationBuilder.CreateTable(
                name: "GameplayData",
                columns: table => new
                {
                    GameplayDataID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    GameplayTime = table.Column<int>(type: "integer", nullable: false),
                    Light = table.Column<float>(type: "real", nullable: false),
                    Money = table.Column<int>(type: "integer", nullable: false),
                    SessionRecordID = table.Column<int>(type: "integer", nullable: false),
                    Speed = table.Column<float>(type: "real", nullable: false),
                    Vision = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameplayData", x => x.GameplayDataID);
                });

            migrationBuilder.CreateTable(
                name: "SessionRecords",
                columns: table => new
                {
                    SessionRecordID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRecords", x => x.SessionRecordID);
                    table.ForeignKey(
                        name: "FK_SessionRecords_AnalysisResults_SessionRecordID",
                        column: x => x.SessionRecordID,
                        principalTable: "AnalysisResults",
                        principalColumn: "AnalysisResultID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRecords_GameplayData_SessionRecordID",
                        column: x => x.SessionRecordID,
                        principalTable: "GameplayData",
                        principalColumn: "GameplayDataID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRecords_Sessions_SessionRecordID",
                        column: x => x.SessionRecordID,
                        principalTable: "Sessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestions_AnalysisResultID",
                table: "AnsweredQuestions",
                column: "AnalysisResultID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnsweredQuestions_AnalysisResults_AnalysisResultID",
                table: "AnsweredQuestions",
                column: "AnalysisResultID",
                principalTable: "AnalysisResults",
                principalColumn: "AnalysisResultID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
