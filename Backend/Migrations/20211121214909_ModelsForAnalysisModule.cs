using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Backend.Migrations
{
    public partial class ModelsForAnalysisModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionRecordID",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AiDifficulty",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnalysisResults",
                columns: table => new
                {
                    AnalysisResultID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DifficultyLevel = table.Column<double>(type: "double precision", nullable: false),
                    ScenarioEnded = table.Column<bool>(type: "boolean", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    Money = table.Column<int>(type: "integer", nullable: false),
                    GameplayTime = table.Column<int>(type: "integer", nullable: false),
                    Light = table.Column<float>(type: "real", nullable: false),
                    Vision = table.Column<float>(type: "real", nullable: false),
                    Speed = table.Column<float>(type: "real", nullable: false),
                    SessionRecordID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameplayData", x => x.GameplayDataID);
                });

            migrationBuilder.CreateTable(
                name: "AnsweredQuestions",
                columns: table => new
                {
                    AnsweredQuestionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionImportanceType = table.Column<int>(type: "integer", nullable: false),
                    QuestionID = table.Column<int>(type: "integer", nullable: true),
                    TimeToAnswer = table.Column<int>(type: "integer", nullable: false),
                    Correctness = table.Column<float>(type: "real", nullable: false),
                    AnalysisResultID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnsweredQuestions", x => x.AnsweredQuestionID);
                    table.ForeignKey(
                        name: "FK_AnsweredQuestions_AnalysisResults_AnalysisResultID",
                        column: x => x.AnalysisResultID,
                        principalTable: "AnalysisResults",
                        principalColumn: "AnalysisResultID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnsweredQuestions_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "AnswerAnsweredQuestion",
                columns: table => new
                {
                    AnsweredAnswersAnswerID = table.Column<int>(type: "integer", nullable: false),
                    AnsweredQuestionsAnsweredQuestionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerAnsweredQuestion", x => new { x.AnsweredAnswersAnswerID, x.AnsweredQuestionsAnsweredQuestionID });
                    table.ForeignKey(
                        name: "FK_AnswerAnsweredQuestion_AnsweredQuestions_AnsweredQuestionsA~",
                        column: x => x.AnsweredQuestionsAnsweredQuestionID,
                        principalTable: "AnsweredQuestions",
                        principalColumn: "AnsweredQuestionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerAnsweredQuestion_Answers_AnsweredAnswersAnswerID",
                        column: x => x.AnsweredAnswersAnswerID,
                        principalTable: "Answers",
                        principalColumn: "AnswerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerAnsweredQuestion_AnsweredQuestionsAnsweredQuestionID",
                table: "AnswerAnsweredQuestion",
                column: "AnsweredQuestionsAnsweredQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestions_AnalysisResultID",
                table: "AnsweredQuestions",
                column: "AnalysisResultID");

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestions_QuestionID",
                table: "AnsweredQuestions",
                column: "QuestionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerAnsweredQuestion");

            migrationBuilder.DropTable(
                name: "SessionRecords");

            migrationBuilder.DropTable(
                name: "AnsweredQuestions");

            migrationBuilder.DropTable(
                name: "GameplayData");

            migrationBuilder.DropTable(
                name: "AnalysisResults");

            migrationBuilder.DropColumn(
                name: "SessionRecordID",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "AiDifficulty",
                table: "Questions");
        }
    }
}
