using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingManyToManyForCountryAndHolidayType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_Countries_CountryCode",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_HolidayDate_HolidayDateId",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_HolidayName_HolidayNameId",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_HolidayTypes_HolidayTypeId",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_HolidayTypes_Countries_CountryCode",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_HolidayTypes_CountryCode",
                table: "HolidayTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolidayName",
                table: "HolidayName");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolidayDate",
                table: "HolidayDate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holiday",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "HolidayTypes");

            migrationBuilder.RenameTable(
                name: "HolidayName",
                newName: "HolidayNames");

            migrationBuilder.RenameTable(
                name: "HolidayDate",
                newName: "HolidayDates");

            migrationBuilder.RenameTable(
                name: "Holiday",
                newName: "Holidays");

            migrationBuilder.RenameIndex(
                name: "IX_Holiday_HolidayTypeId",
                table: "Holidays",
                newName: "IX_Holidays_HolidayTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Holiday_HolidayNameId",
                table: "Holidays",
                newName: "IX_Holidays_HolidayNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Holiday_HolidayDateId",
                table: "Holidays",
                newName: "IX_Holidays_HolidayDateId");

            migrationBuilder.RenameIndex(
                name: "IX_Holiday_CountryCode",
                table: "Holidays",
                newName: "IX_Holidays_CountryCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolidayNames",
                table: "HolidayNames",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolidayDates",
                table: "HolidayDates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holidays",
                table: "Holidays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Country_HolidayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HolidayTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country_HolidayTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Country_HolidayTypes_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Country_HolidayTypes_HolidayTypes_HolidayTypeId",
                        column: x => x.HolidayTypeId,
                        principalTable: "HolidayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_HolidayTypes_CountryCode",
                table: "Country_HolidayTypes",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Country_HolidayTypes_HolidayTypeId",
                table: "Country_HolidayTypes",
                column: "HolidayTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Countries_CountryCode",
                table: "Holidays",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Countries_CountryCode",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayDates_HolidayDateId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayNames_HolidayNameId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_HolidayTypes_HolidayTypeId",
                table: "Holidays");

            migrationBuilder.DropTable(
                name: "Country_HolidayTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holidays",
                table: "Holidays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolidayNames",
                table: "HolidayNames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HolidayDates",
                table: "HolidayDates");

            migrationBuilder.RenameTable(
                name: "Holidays",
                newName: "Holiday");

            migrationBuilder.RenameTable(
                name: "HolidayNames",
                newName: "HolidayName");

            migrationBuilder.RenameTable(
                name: "HolidayDates",
                newName: "HolidayDate");

            migrationBuilder.RenameIndex(
                name: "IX_Holidays_HolidayTypeId",
                table: "Holiday",
                newName: "IX_Holiday_HolidayTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Holidays_HolidayNameId",
                table: "Holiday",
                newName: "IX_Holiday_HolidayNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Holidays_HolidayDateId",
                table: "Holiday",
                newName: "IX_Holiday_HolidayDateId");

            migrationBuilder.RenameIndex(
                name: "IX_Holidays_CountryCode",
                table: "Holiday",
                newName: "IX_Holiday_CountryCode");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "HolidayTypes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holiday",
                table: "Holiday",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolidayName",
                table: "HolidayName",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HolidayDate",
                table: "HolidayDate",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTypes_CountryCode",
                table: "HolidayTypes",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Countries_CountryCode",
                table: "Holiday",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_HolidayDate_HolidayDateId",
                table: "Holiday",
                column: "HolidayDateId",
                principalTable: "HolidayDate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_HolidayName_HolidayNameId",
                table: "Holiday",
                column: "HolidayNameId",
                principalTable: "HolidayName",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_HolidayTypes_HolidayTypeId",
                table: "Holiday",
                column: "HolidayTypeId",
                principalTable: "HolidayTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayTypes_Countries_CountryCode",
                table: "HolidayTypes",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
