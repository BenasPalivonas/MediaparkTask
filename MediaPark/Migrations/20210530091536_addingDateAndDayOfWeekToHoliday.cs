using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class addingDateAndDayOfWeekToHoliday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayDates");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Holidays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfTheWeek",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "DayOfTheWeek",
                table: "Holidays");

            migrationBuilder.CreateTable(
                name: "HolidayDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    HolidayId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayDates_Holidays_HolidayId",
                        column: x => x.HolidayId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayDates_HolidayId",
                table: "HolidayDates",
                column: "HolidayId",
                unique: true);
        }
    }
}
