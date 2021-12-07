using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Product
{
    internal class DevolucaoVendaMap : IEntityTypeConfiguration<DevolucaoVenda>
    {
        public void Configure(EntityTypeBuilder<DevolucaoVenda> builder)
        {
            builder.ToTable("DevolucaoVenda");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Motivo).HasMaxLength(1000);
            builder.Property(t => t.IsParcial).HasDefaultValue(false);

            builder.HasOne(t => t.Competencia).WithMany(t => t.DevolucoesVenda).HasForeignKey(t => t.CompetenciaId);
            builder.HasOne(t => t.Produto).WithMany(t => t.DevolucoesVenda).HasForeignKey(t => t.ProdutoId);
            builder.HasOne(t => t.VendaProduto).WithMany(t => t.DevolucoesVenda).HasForeignKey(t => t.VendaProdutoId);
        }
    }
}
