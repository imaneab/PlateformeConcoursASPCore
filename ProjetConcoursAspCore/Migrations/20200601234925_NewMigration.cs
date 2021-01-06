using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetConcoursAspCore.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_Cin",
                table: "Etudiants",
                column: "Cin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_Cne",
                table: "Etudiants",
                column: "Cne",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_Email",
                table: "Etudiants",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Etudiants_Cin",
                table: "Etudiants");

            migrationBuilder.DropIndex(
                name: "IX_Etudiants_Cne",
                table: "Etudiants");

            migrationBuilder.DropIndex(
                name: "IX_Etudiants_Email",
                table: "Etudiants");
        }
    }
}
