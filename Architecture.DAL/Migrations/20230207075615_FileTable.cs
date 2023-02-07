using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Architecture.DAL.Migrations
{
    public partial class FileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 173, DateTimeKind.Local).AddTicks(9084),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 19, 16, 56, 50, 629, DateTimeKind.Local).AddTicks(1221));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 170, DateTimeKind.Local).AddTicks(3398),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 19, 16, 56, 50, 625, DateTimeKind.Local).AddTicks(1180));

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AbsolutePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 176, DateTimeKind.Local).AddTicks(4868)),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 176, DateTimeKind.Local).AddTicks(5201))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileName",
                table: "Files",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 19, 16, 56, 50, 629, DateTimeKind.Local).AddTicks(1221),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 173, DateTimeKind.Local).AddTicks(9084));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 19, 16, 56, 50, 625, DateTimeKind.Local).AddTicks(1180),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 7, 10, 56, 15, 170, DateTimeKind.Local).AddTicks(3398));
        }
    }
}
