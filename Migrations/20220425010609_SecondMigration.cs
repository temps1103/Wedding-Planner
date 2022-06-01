using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wedding_Planner.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Weddings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "Weddings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
