using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Account
{
    internal class PagamentoVendaMap : IEntityTypeConfiguration<PagamentoVenda>
    {
        public void Configure(EntityTypeBuilder<PagamentoVenda> builder)
        {
            builder.ToTable("PagamentoVenda");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.DiaVencimentoParcela).HasDefaultValue(1);
            
            builder.HasOne(t => t.Competencia).WithMany(t=>t.PagamentosVendas).HasForeignKey(t=>t.CompetenciaId);
            builder.HasOne(t => t.Venda).WithMany(t => t.PagamentosVendas).HasForeignKey(t => t.VendaId);

        }
    }
}
