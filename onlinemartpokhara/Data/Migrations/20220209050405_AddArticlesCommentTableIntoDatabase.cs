using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlinemartpokhara.Data.Migrations
{
    public partial class AddArticlesCommentTableIntoDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticlesComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThisDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_ArticlesComments_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesComments_ProductsId",
                table: "ArticlesComments",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticlesComments");
        }
    }
}
