using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AnswereQuestionAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnsweredQuestions_Questions_QuestionID",
                table: "AnsweredQuestions");

            migrationBuilder.DropIndex(
                name: "IX_AnsweredQuestions_QuestionID",
                table: "AnsweredQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionID",
                table: "AnsweredQuestions");

            migrationBuilder.AddColumn<byte>(
                name: "Difficulty",
                table: "AnsweredQuestions",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionIdRef",
                table: "AnsweredQuestions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "AnsweredQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionIdRef",
                table: "AnsweredQuestions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionID",
                table: "AnsweredQuestions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestions_QuestionID",
                table: "AnsweredQuestions",
                column: "QuestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnsweredQuestions_Questions_QuestionID",
                table: "AnsweredQuestions",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "QuestionID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
