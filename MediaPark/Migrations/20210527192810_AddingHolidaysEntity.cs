using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingHolidaysEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayDateId = table.Column<int>(type: "int", nullable: true),
                    HolidayNameId = table.Column<int>(type: "int", nullable: true),
                    HolidayTypeId = table.Column<int>(type: "int", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holiday_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holiday_HolidayDate_HolidayDateId",
                        column: x => x.HolidayDateId,
                        principalTable: "HolidayDate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holiday_HolidayName_HolidayNameId",
                        column: x => x.HolidayNameId,
                        principalTable: "HolidayName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holiday_HolidayTypes_HolidayTypeId",
                        column: x => x.HolidayTypeId,
                        principalTable: "HolidayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_CountryCode",
                table: "Holiday",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayDateId",
                table: "Holiday",
                column: "HolidayDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayNameId",
                table: "Holiday",
                column: "HolidayNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayTypeId",
                table: "Holiday",
                column: "HolidayTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "HolidayDate");

            migrationBuilder.DropTable(
                name: "HolidayName");
        }
    }
}
