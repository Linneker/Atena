using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class CorrigindoVendaProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValorProduto_TipoValorProduto_ProdutoId",
                table: "ValorProduto");

            migrationBuilder.DropForeignKey(
                name: "FK_VendaProduto_Venda_ProdutoId",
                table: "VendaProduto");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorPago",
                table: "Venda",
                type: "decimal(18, 2)",
                precision: 24,
                scale: 4,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendaProduto_VendaId",
                table: "VendaProduto",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_ValorProduto_TipoValorProdutoId",
                table: "ValorProduto",
                column: "TipoValorProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ValorProduto_TipoValorProduto_TipoValorProdutoId",
                table: "ValorProduto",
                column: "TipoValorProdutoId",
                principalTable: "TipoValorProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProduto_Venda_VendaId",
                table: "VendaProduto",
                column: "VendaId",
                principalTable: "Venda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValorProduto_TipoValorProduto_TipoValorProdutoId",
                table: "ValorProduto");

            migrationBuilder.DropForeignKey(
                name: "FK_VendaProduto_Venda_VendaId",
                table: "VendaProduto");

            migrationBuilder.DropIndex(
                name: "IX_VendaProduto_VendaId",
                table: "VendaProduto");

            migrationBuilder.DropIndex(
                name: "IX_ValorProduto_TipoValorProdutoId",
                table: "ValorProduto");

            migrationBuilder.DropColumn(
                name: "ValorPago",
                table: "Venda");

            migrationBuilder.AddForeignKey(
                name: "FK_ValorProduto_TipoValorProduto_ProdutoId",
                table: "ValorProduto",
                column: "ProdutoId",
                principalTable: "TipoValorProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendaProduto_Venda_ProdutoId",
                table: "VendaProduto",
                column: "ProdutoId",
                principalTable: "Venda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
