using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class AddNumeroDaParcela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroDaParcela",
                table: "Pagamento",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroDaParcela",
                table: "Pagamento");
        }
    }
}
