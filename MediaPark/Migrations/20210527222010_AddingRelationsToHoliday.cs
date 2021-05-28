using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingRelationsToHoliday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holiday_HolidayTypes");

            migrationBuilder.AddColumn<int>(
                name: "HolidayTypeId",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_HolidayTypeId",
                table: "Holidays",
                column: "HolidayTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_HolidayTypes_HolidayTypeId",
                table: "Holidays",
                column: "HolidayTypeId",
                principalTable: "HolidayTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayTypes_HolidayTypeId",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_HolidayTypeId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "HolidayTypeId",
                table: "Holidays");

            migrationBuilder.CreateTable(
                name: "Holiday_HolidayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayId = table.Column<int>(type: "int", nullable: false),
                    HolidayTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday_HolidayTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holiday_HolidayTypes_Holidays_HolidayId",
                        column: x => x.HolidayId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Holiday_HolidayTypes_HolidayTypes_HolidayTypeId",
                        column: x => x.HolidayTypeId,
                        principalTable: "HolidayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayTypes_HolidayId",
                table: "Holiday_HolidayTypes",
                column: "HolidayId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayTypes_HolidayTypeId",
                table: "Holiday_HolidayTypes",
                column: "HolidayTypeId");
        }
    }
}
