using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BulletinBoard.DB.Migrations
{
    public partial class New_Column_Bulletin_Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Test",
                table: "Bulletins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Bulletins");
        }
    }
}
