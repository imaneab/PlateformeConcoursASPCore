using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetConcoursAspCore.Migrations
{
    public partial class addcolumn_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "picture",
                table: "Etudiants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "picture",
                table: "Etudiants");
        }
    }
}
