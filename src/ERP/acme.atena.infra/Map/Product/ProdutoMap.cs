using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Product
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Descricao).HasMaxLength(2000);
            builder.Property(t => t.Validade).IsRequired(false);

            builder.HasOne(t => t.CodigoProduto).WithMany(t=>t.Produtos).HasForeignKey(t=>t.Codigo);

            builder.HasOne(t => t.TipoProduto).WithMany(t => t.Produtos).HasForeignKey(t => t.TipoProdutoId);
        }
    }
}
