using Microsoft.EntityFrameworkCore.Migrations;

namespace YellowNotes.Core.Migrations
{
    public partial class UpdateDatabaseContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category",
                table: "Notes");

            migrationBuilder.AddForeignKey(
                name: "FK_Category",
                table: "Notes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category",
                table: "Notes");

            migrationBuilder.AddForeignKey(
                name: "FK_Category",
                table: "Notes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
