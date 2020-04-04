using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YellowNotes.Core.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserEmail",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Variant",
                table: "Notes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(4) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 4);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Notes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User",
                table: "Notes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Variant",
                table: "Notes",
                type: "varchar(4) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Notes",
                type: "varchar(255) CHARACTER SET utf8mb4",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserEmail",
                table: "Notes",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_User",
                table: "Notes",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
