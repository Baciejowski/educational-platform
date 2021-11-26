using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class SessionAnsweredQuestionsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionID",
                table: "AnsweredQuestions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnsweredQuestions_SessionID",
                table: "AnsweredQuestions",
                column: "SessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnsweredQuestions_Sessions_SessionID",
                table: "AnsweredQuestions",
                column: "SessionID",
                principalTable: "Sessions",
                principalColumn: "SessionID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnsweredQuestions_Sessions_SessionID",
                table: "AnsweredQuestions");

            migrationBuilder.DropIndex(
                name: "IX_AnsweredQuestions_SessionID",
                table: "AnsweredQuestions");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "AnsweredQuestions");
        }
    }
}
