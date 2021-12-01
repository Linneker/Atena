using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Account
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.DataCredito);
            builder.Property(t => t.DataPagamento);
            builder.Property(t => t.EnumFormaPagamento);
            builder.Property(t => t.EnumTipoPagamento);
            builder.Property(t => t.NumeroDeParcela).HasDefaultValue(1);
            builder.Property(t => t.Parcelado).HasDefaultValue(false);
            builder.Property(t => t.ValorPago);

            builder.HasOne(t => t.Usuario).WithMany(t => t.Pagamentos).HasForeignKey(t => t.ClienteId);
            builder.HasOne(t => t.Cliente).WithMany(t => t.Pagamentos).HasForeignKey(t => t.PessoaId);
            builder.HasOne(t => t.Fornecedor).WithMany(t => t.Pagamentos).HasForeignKey(t => t.FornecedorId);
            builder.HasOne(t => t.Competencia).WithMany(t => t.Pagamentos).HasForeignKey(t => t.CompetenciaId);
        }
    }
}
