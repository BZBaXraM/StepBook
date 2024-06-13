using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepBook.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialWithEmailUserProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");
        }
    }
}
