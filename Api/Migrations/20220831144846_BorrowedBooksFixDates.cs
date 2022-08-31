using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class BorrowedBooksFixDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LIMIT DATE",
                table: "BORROWED BOOK",
                type: "date",
                nullable: false,
                defaultValueSql: "DATEADD(DAY, 30, GETUTCDATE())",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BORROWED DATE",
                table: "BORROWED BOOK",
                type: "date",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LIMIT DATE",
                table: "BORROWED BOOK",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "DATEADD(DAY, 30, GETUTCDATE())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BORROWED DATE",
                table: "BORROWED BOOK",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
