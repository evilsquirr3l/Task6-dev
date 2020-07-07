using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    ReaderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReaderProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReaderId = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReaderProfiles_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(nullable: false),
                    CardId = table.Column<int>(nullable: false),
                    TakeDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Oscar Wilde", "The Picture of Dorian Gray", 1890 },
                    { 2, "Jack London", "White fang", 1906 },
                    { 3, "Daniel Defo", "Robinson Crusoe", 1719 },
                    { 4, "Ernest Hemingway", "The Old Man and the Sea", 1952 },
                    { 5, "George R. R. Martin", "A Dance with Dragons", 2011 }
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "serhii_email@gmail.com", "Serhii" },
                    { 2, "ivan_email@gmail.com", "Ivan" },
                    { 3, "petro_email@gmail.com", "Petro" },
                    { 4, "oleksandr_email@gmail.com", "Oleksandr" }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Created", "ReaderId" },
                values: new object[,]
                {
                    { 1, new DateTime(2016, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2017, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, new DateTime(2018, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, new DateTime(2016, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, new DateTime(2020, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.InsertData(
                table: "ReaderProfiles",
                columns: new[] { "Id", "Address", "Phone", "ReaderId" },
                values: new object[,]
                {
                    { 1, "Kyiv, 1", "123456789", 1 },
                    { 2, "Kyiv, 2", "456789123", 2 },
                    { 3, "Kyiv, 3", "789123456", 3 },
                    { 4, "Kyiv, 4", "326159487", 4 }
                });

            migrationBuilder.InsertData(
                table: "Histories",
                columns: new[] { "Id", "BookId", "CardId", "ReturnDate", "TakeDate" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2016, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, 1, new DateTime(2018, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3, 1, null, new DateTime(2020, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, 2, new DateTime(2020, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 4, 3, null, new DateTime(2020, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 4, new DateTime(2016, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 4, new DateTime(2016, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 5, 5, null, new DateTime(2020, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ReaderId",
                table: "Cards",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_BookId",
                table: "Histories",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_CardId",
                table: "Histories",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_ReaderProfiles_ReaderId",
                table: "ReaderProfiles",
                column: "ReaderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "ReaderProfiles");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Readers");
        }
    }
}
