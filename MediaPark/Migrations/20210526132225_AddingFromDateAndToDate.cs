using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaPark.Migrations
{
    public partial class AddingFromDateAndToDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Counntries",
                table: "Counntries");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Counntries");

            migrationBuilder.RenameTable(
                name: "Counntries",
                newName: "Countries");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Countries",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "CountryCode");

            migrationBuilder.CreateTable(
                name: "FromDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FromDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDates", x => x.Id);
                });

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
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_ToDates_ToDateId",
                table: "Countries",
                column: "ToDateId",
                principalTable: "ToDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_FromDates_FromDateId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_ToDates_ToDateId",
                table: "Countries");

            migrationBuilder.DropTable(
                name: "FromDates");

            migrationBuilder.DropTable(
                name: "ToDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_FromDateId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_ToDateId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FromDateId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ToDateId",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Counntries");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Counntries",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Counntries",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Counntries",
                table: "Counntries",
                column: "id");
        }
    }
}
