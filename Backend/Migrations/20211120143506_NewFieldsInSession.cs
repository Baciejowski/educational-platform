using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class NewFieldsInSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RandomCategorization",
                table: "Sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RandomTest",
                table: "Sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RandomCategorization",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RandomTest",
                table: "Sessions");
        }
    }
}
