using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class RenameSessionProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RandomCategorization",
                table: "Sessions",
                newName: "AiCategorization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AiCategorization",
                table: "Sessions",
                newName: "RandomCategorization");
        }
    }
}
