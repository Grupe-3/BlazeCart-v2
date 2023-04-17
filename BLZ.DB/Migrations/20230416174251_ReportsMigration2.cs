using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BLZ.DB.Migrations
{
    /// <inheritdoc />
    public partial class ReportsMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Reports");

            migrationBuilder.AddColumn<bool>(
                name: "IsSolved",
                table: "Reports",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpam",
                table: "Reports",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSolved",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsSpam",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Reports",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
