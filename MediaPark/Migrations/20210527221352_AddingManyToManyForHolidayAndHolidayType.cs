using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingManyToManyForHolidayAndHolidayType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayDates_HolidayDateId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayNames_HolidayNameId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayTypes_HolidayTypeId",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_HolidayDateId",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_HolidayNameId",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_HolidayTypeId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "HolidayDateId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "HolidayNameId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "HolidayTypeId",
                table: "Holidays");

            migrationBuilder.AddColumn<int>(
                name: "HolidayId",
                table: "HolidayNames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HolidayId",
                table: "HolidayDates",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames",
                column: "HolidayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayDates_HolidayId",
                table: "HolidayDates",
                column: "HolidayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayTypes_HolidayId",
                table: "Holiday_HolidayTypes",
                column: "HolidayId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayTypes_HolidayTypeId",
                table: "Holiday_HolidayTypes",
                column: "HolidayTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayDates_Holidays_HolidayId",
                table: "HolidayDates",
                column: "HolidayId",
                principalTable: "Holidays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayNames_Holidays_HolidayId",
                table: "HolidayNames",
                column: "HolidayId",
                principalTable: "Holidays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolidayDates_Holidays_HolidayId",
                table: "HolidayDates");

            migrationBuilder.DropForeignKey(
                name: "FK_HolidayNames_Holidays_HolidayId",
                table: "HolidayNames");

            migrationBuilder.DropTable(
                name: "Holiday_HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_HolidayNames_HolidayId",
                table: "HolidayNames");

            migrationBuilder.DropIndex(
                name: "IX_HolidayDates_HolidayId",
                table: "HolidayDates");

            migrationBuilder.DropColumn(
                name: "HolidayId",
                table: "HolidayNames");

            migrationBuilder.DropColumn(
                name: "HolidayId",
                table: "HolidayDates");

            migrationBuilder.AddColumn<int>(
                name: "HolidayDateId",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HolidayNameId",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HolidayTypeId",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_HolidayDateId",
                table: "Holidays",
                column: "HolidayDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_HolidayNameId",
                table: "Holidays",
                column: "HolidayNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_HolidayTypeId",
                table: "Holidays",
                column: "HolidayTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_HolidayDates_HolidayDateId",
                table: "Holidays",
                column: "HolidayDateId",
                principalTable: "HolidayDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_HolidayNames_HolidayNameId",
                table: "Holidays",
                column: "HolidayNameId",
                principalTable: "HolidayNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_HolidayTypes_HolidayTypeId",
                table: "Holidays",
                column: "HolidayTypeId",
                principalTable: "HolidayTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
