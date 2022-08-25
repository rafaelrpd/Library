using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AUTHOR",
            //    columns: table => new
            //    {
            //        AUTHORID = table.Column<int>(name: "AUTHOR ID", type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //        REGISTRATIONDATE = table.Column<DateTime>(name: "REGISTRATION DATE", type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AUTHOR", x => x.AUTHORID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CATEGORY",
            //    columns: table => new
            //    {
            //        CATEGORYID = table.Column<int>(name: "CATEGORY ID", type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CATEGORY", x => x.CATEGORYID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CLIENT",
            //    columns: table => new
            //    {
            //        CPF = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
            //        NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //        REGISTRATIONDATE = table.Column<DateTime>(name: "REGISTRATION DATE", type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CLIENT_ID", x => x.CPF);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BOOK",
            //    columns: table => new
            //    {
            //        ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
            //        AUTHORID = table.Column<int>(name: "AUTHOR ID", type: "int", nullable: false),
            //        CATEGORYID = table.Column<int>(name: "CATEGORY ID", type: "int", nullable: false),
            //        TITLE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //        QUANTITY = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BOOK_ID", x => x.ISBN);
            //        table.ForeignKey(
            //            name: "FK_AUTHOR_ID",
            //            column: x => x.AUTHORID,
            //            principalTable: "AUTHOR",
            //            principalColumn: "AUTHOR ID");
            //        table.ForeignKey(
            //            name: "FK_CATEGORY_ID",
            //            column: x => x.CATEGORYID,
            //            principalTable: "CATEGORY",
            //            principalColumn: "CATEGORY ID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BORROWED BOOK",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CLIENTID = table.Column<string>(name: "CLIENT ID", type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
            //        BOOKID = table.Column<string>(name: "BOOK ID", type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
            //        BORROWEDDATE = table.Column<DateTime>(name: "BORROWED DATE", type: "date", nullable: true),
            //        LIMITDATE = table.Column<DateTime>(name: "LIMIT DATE", type: "date", nullable: true),
            //        RETURNEDDATE = table.Column<DateTime>(name: "RETURNED DATE", type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BORROWED BOOK", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_BOOK_ID",
            //            column: x => x.BOOKID,
            //            principalTable: "BOOK",
            //            principalColumn: "ISBN");
            //        table.ForeignKey(
            //            name: "FK_CLIENT_ID",
            //            column: x => x.CLIENTID,
            //            principalTable: "CLIENT",
            //            principalColumn: "CPF");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_BOOK_AUTHOR ID",
            //    table: "BOOK",
            //    column: "AUTHOR ID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BOOK_CATEGORY ID",
            //    table: "BOOK",
            //    column: "CATEGORY ID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BORROWED BOOK_BOOK ID",
            //    table: "BORROWED BOOK",
            //    column: "BOOK ID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BORROWED BOOK_CLIENT ID",
            //    table: "BORROWED BOOK",
            //    column: "CLIENT ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BORROWED BOOK");

            //migrationBuilder.DropTable(
            //    name: "BOOK");

            //migrationBuilder.DropTable(
            //    name: "CLIENT");

            //migrationBuilder.DropTable(
            //    name: "AUTHOR");

            //migrationBuilder.DropTable(
            //    name: "CATEGORY");
        }
    }
}
