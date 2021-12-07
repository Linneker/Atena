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
    internal class EntradaProdutoEstoqueMap : IEntityTypeConfiguration<EntradaProdutoEstoque>
    {
        public void Configure(EntityTypeBuilder<EntradaProdutoEstoque> builder)
        {
            builder.ToTable("EntradaProdutoEstoque");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.DataEntrada).HasDefaultValueSql("NOW()");

            builder.HasOne(t => t.Competencia).WithMany(t => t.EntradaProdutoEstoques).HasForeignKey(t => t.CompetenciaId);
            builder.HasOne(t => t.Produto).WithMany(t => t.EntradaProdutoEstoques).HasForeignKey(t => t.ProdutoId);
            builder.HasOne(t => t.Estoque).WithMany(t => t.EntradaProdutoEstoques).HasForeignKey(t => t.EstoqueId);
        }
    }
}