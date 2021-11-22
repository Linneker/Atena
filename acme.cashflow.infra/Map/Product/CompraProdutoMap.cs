using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Product
{
    public class CompraProdutoMap : IEntityTypeConfiguration<CompraProduto>
    {
        public void Configure(EntityTypeBuilder<CompraProduto> builder)
        {
            builder.ToTable("CompraProduto");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Produto).WithMany(t => t.ComprasProdutos).HasForeignKey(t => t.ProdutoId);
            builder.HasOne(t => t.Compra).WithMany(t => t.ComprasProdutos).HasForeignKey(t => t.CompraId);

        }
    }
}
