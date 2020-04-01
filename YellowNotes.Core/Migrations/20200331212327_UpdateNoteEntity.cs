using Microsoft.EntityFrameworkCore.Migrations;

namespace YellowNotes.Core.Migrations
{
    public partial class UpdateNoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Variant",
                table: "Notes",
                unicode: false,
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldUnicode: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notes",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Variant",
                table: "Notes",
                type: "longtext CHARACTER SET utf8mb4",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 4);
        }
    }
}
