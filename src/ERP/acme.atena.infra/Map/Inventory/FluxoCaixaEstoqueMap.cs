using acme.atena.domain.DTO.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Inventory
{
    public class FluxoCaixaEstoqueMap: BaseMap<FluxoCaixaEstoque>
    {
        public override void Configure(EntityTypeBuilder<FluxoCaixaEstoque> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(FluxoCaixaEstoque));

            builder.Property(t => t.ValorTotalEntrada);
            builder.Property(t => t.ValorTotalEstoqueFinal);
            builder.Property(t => t.ValorTotalEstoqueInicial);

            builder.HasOne(t => t.Competencia).WithMany(t => t.FluxoCaixaEstoques).HasForeignKey(t => t.CompetenciaId);

            builder.Property(t => t.ValorTotalSaida);

        }
    }
}
