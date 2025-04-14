using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodiaBG.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToInstruments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Instruments_CategoryId",
                table: "Instruments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instruments_Categories_CategoryId",
                table: "Instruments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instruments_Categories_CategoryId",
                table: "Instruments");

            migrationBuilder.DropIndex(
                name: "IX_Instruments_CategoryId",
                table: "Instruments");
        }
    }
}
