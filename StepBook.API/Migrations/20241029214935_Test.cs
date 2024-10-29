using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepBook.API.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlackListedUser_Users_BlackListedUserId",
                table: "BlackListedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BlackListedUser_Users_UserId",
                table: "BlackListedUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlackListedUser",
                table: "BlackListedUser");

            migrationBuilder.RenameTable(
                name: "BlackListedUser",
                newName: "BlackListedUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BlackListedUser_BlackListedUserId",
                table: "BlackListedUsers",
                newName: "IX_BlackListedUsers_BlackListedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlackListedUsers",
                table: "BlackListedUsers",
                columns: new[] { "UserId", "BlackListedUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlackListedUsers_Users_BlackListedUserId",
                table: "BlackListedUsers",
                column: "BlackListedUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlackListedUsers_Users_UserId",
                table: "BlackListedUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlackListedUsers_Users_BlackListedUserId",
                table: "BlackListedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlackListedUsers_Users_UserId",
                table: "BlackListedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlackListedUsers",
                table: "BlackListedUsers");

            migrationBuilder.RenameTable(
                name: "BlackListedUsers",
                newName: "BlackListedUser");

            migrationBuilder.RenameIndex(
                name: "IX_BlackListedUsers_BlackListedUserId",
                table: "BlackListedUser",
                newName: "IX_BlackListedUser_BlackListedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlackListedUser",
                table: "BlackListedUser",
                columns: new[] { "UserId", "BlackListedUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlackListedUser_Users_BlackListedUserId",
                table: "BlackListedUser",
                column: "BlackListedUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlackListedUser_Users_UserId",
                table: "BlackListedUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
