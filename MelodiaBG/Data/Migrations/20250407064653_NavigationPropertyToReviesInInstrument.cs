using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodiaBG.Data.Migrations
{
    /// <inheritdoc />
    public partial class NavigationPropertyToReviesInInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstrumentId1",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_InstrumentId1",
                table: "Reviews",
                column: "InstrumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Instruments_InstrumentId1",
                table: "Reviews",
                column: "InstrumentId1",
                principalTable: "Instruments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Instruments_InstrumentId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_InstrumentId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "InstrumentId1",
                table: "Reviews");
        }
    }
}
