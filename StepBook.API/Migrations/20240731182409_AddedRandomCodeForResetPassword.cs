using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepBook.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedRandomCodeForResetPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RandomCode",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomCode",
                table: "Users");
        }
    }
}
