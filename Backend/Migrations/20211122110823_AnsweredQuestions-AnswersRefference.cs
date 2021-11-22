using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AnsweredQuestionsAnswersRefference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerAnsweredQuestion");

            migrationBuilder.AddColumn<List<int>>(
                name: "AnsweredAnswers",
                table: "AnsweredQuestions",
                type: "integer[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsweredAnswers",
                table: "AnsweredQuestions");

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
        }
    }
}
