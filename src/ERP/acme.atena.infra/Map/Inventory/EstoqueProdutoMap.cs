using acme.atena.domain.DTO.Enum;
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
    internal class EstoqueProdutoMap : IEntityTypeConfiguration<EstoqueProduto>
    {
        public void Configure(EntityTypeBuilder<EstoqueProduto> builder)
        {
            builder.ToTable("EstoqueProduto");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.QuantidadeProduto).IsRequired();
            builder.Property(t => t.QuantidadeMaxima).IsRequired();
            builder.Property(t => t.QuantidadeMinima).IsRequired();

            builder.HasOne(t => t.Produto).WithMany(t => t.EstoqueProdutos).HasForeignKey(t => t.ProdutoId);
            builder.HasOne(t => t.Estoque).WithMany(t => t.EstoqueProdutos).HasForeignKey(t => t.EstoqueId);

        }
    }
}
