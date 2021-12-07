using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class RemovendoNumeroDeParcela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroDeParcela",
                table: "Pagamento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroDeParcela",
                table: "Pagamento",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }
    }
}
