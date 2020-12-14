using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoTeste.Migrations
{
    public partial class ForeignKey1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Listas_ListasId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Listas_ListasId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_ListasId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_ListasId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ListasId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ListasId",
                table: "Clientes");

            migrationBuilder.CreateIndex(
                name: "IX_Listas_ClienteId",
                table: "Listas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Listas_ProdutoId",
                table: "Listas",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listas_Clientes_ClienteId",
                table: "Listas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listas_Produtos_ProdutoId",
                table: "Listas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listas_Clientes_ClienteId",
                table: "Listas");

            migrationBuilder.DropForeignKey(
                name: "FK_Listas_Produtos_ProdutoId",
                table: "Listas");

            migrationBuilder.DropIndex(
                name: "IX_Listas_ClienteId",
                table: "Listas");

            migrationBuilder.DropIndex(
                name: "IX_Listas_ProdutoId",
                table: "Listas");

            migrationBuilder.AddColumn<int>(
                name: "ListasId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListasId",
                table: "Clientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_ListasId",
                table: "Produtos",
                column: "ListasId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ListasId",
                table: "Clientes",
                column: "ListasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Listas_ListasId",
                table: "Clientes",
                column: "ListasId",
                principalTable: "Listas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Listas_ListasId",
                table: "Produtos",
                column: "ListasId",
                principalTable: "Listas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
