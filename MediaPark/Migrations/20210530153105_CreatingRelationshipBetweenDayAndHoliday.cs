using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class CreatingRelationshipBetweenDayAndHoliday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HolidayId",
                table: "Days",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_DayId",
                table: "Holidays",
                column: "DayId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Days_DayId",
                table: "Holidays",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Days_DayId",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_DayId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "HolidayId",
                table: "Days");
        }
    }
}
