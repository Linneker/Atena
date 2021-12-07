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
    internal class PagamentoFormaPagamentoMap : IEntityTypeConfiguration<PagamentoFormaPagamento>
    {
        public void Configure(EntityTypeBuilder<PagamentoFormaPagamento> builder)
        {
            builder.ToTable("PagamentoFormaPagamento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Pagamento).WithMany(t => t.Pagamentos).HasForeignKey(t => t.PagamentoId);
            builder.HasOne(t => t.FormaPagamento).WithMany(t => t.PagamentoFormaPagamentos).HasForeignKey(t => t.FormaPagamentoId);

        }
    }
}
