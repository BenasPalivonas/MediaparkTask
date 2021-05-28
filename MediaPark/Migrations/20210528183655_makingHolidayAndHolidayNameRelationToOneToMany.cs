using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class makingHolidayAndHolidayNameRelationToOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames",
                column: "HolidayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames",
                column: "HolidayId",
                unique: true);
        }
    }
}
