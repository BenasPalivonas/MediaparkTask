using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingForeignKeyToFromDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_FromDates_FromDateId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_ToDates_ToDateId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_FromDateId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_ToDateId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FromDateId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ToDateId",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "ToDates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "FromDates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDates_CountryCode",
                table: "ToDates",
                column: "CountryCode",
                unique: true,
                filter: "[CountryCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FromDates_CountryCode",
                table: "FromDates",
                column: "CountryCode",
                unique: true,
                filter: "[CountryCode] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FromDates_Countries_CountryCode",
                table: "FromDates",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDates_Countries_CountryCode",
                table: "ToDates",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FromDates_Countries_CountryCode",
                table: "FromDates");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDates_Countries_CountryCode",
                table: "ToDates");

            migrationBuilder.DropIndex(
                name: "IX_ToDates_CountryCode",
                table: "ToDates");

            migrationBuilder.DropIndex(
                name: "IX_FromDates_CountryCode",
                table: "FromDates");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "ToDates");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "FromDates");

            migrationBuilder.AddColumn<int>(
                name: "FromDateId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToDateId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_FromDateId",
                table: "Countries",
                column: "FromDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ToDateId",
                table: "Countries",
                column: "ToDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_FromDates_FromDateId",
                table: "Countries",
                column: "FromDateId",
                principalTable: "FromDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_ToDates_ToDateId",
                table: "Countries",
                column: "ToDateId",
                principalTable: "ToDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
