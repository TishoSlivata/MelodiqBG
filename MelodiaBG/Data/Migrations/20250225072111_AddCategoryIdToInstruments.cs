using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodiaBG.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToInstruments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Instruments");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Instruments",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Instruments");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Instruments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
