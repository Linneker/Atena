using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Product
{
    public class VendaProdutoMap : IEntityTypeConfiguration<VendaProduto>
    {
        public void Configure(EntityTypeBuilder<VendaProduto> builder)
        {
            builder.ToTable("VendaProduto");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Produto).WithMany(t => t.VendasProdutos).HasForeignKey(t => t.ProdutoId);
            builder.HasOne(t => t.Venda).WithMany(t => t.VendasProdutos).HasForeignKey(t => t.ProdutoId);

        }
    }
}
