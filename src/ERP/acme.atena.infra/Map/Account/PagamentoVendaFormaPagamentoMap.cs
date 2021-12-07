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
    internal class PagamentoVendaFormaPagamentoMap : IEntityTypeConfiguration<PagamentoVendaFormaPagamento>
    {
        public void Configure(EntityTypeBuilder<PagamentoVendaFormaPagamento> builder)
        {

            builder.ToTable("PagamentoVendaFormaPagamento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.PagamentoVenda).WithMany(t => t.PagamentoVendasFormasPagamentos).HasForeignKey(t => t.PagamentoVendaId);
            builder.HasOne(t => t.FormaPagamento).WithMany(t => t.PagamentoVendasFormasPagamentos).HasForeignKey(t => t.FormaPagamentoId);


        }
    }
}
