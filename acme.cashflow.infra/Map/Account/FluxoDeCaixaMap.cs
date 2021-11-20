using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Account
{
    public class FluxoDeCaixaMap : IEntityTypeConfiguration<FluxoDeCaixa>
    {
        public void Configure(EntityTypeBuilder<FluxoDeCaixa> builder)
        {
            builder.ToTable("FluxoDeCaixa");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.SaldoFinalCaixa);
            builder.Property(t => t.SaldoInicial);
            builder.Property(t => t.SaldoOperacional);

            builder.HasOne(t => t.Competencia).WithMany(t=>t.FluxosDeCasas).HasForeignKey(t=>t.CompetenciaId);

            builder.Property(t => t.SaldoFinalCaixa);
        }
    }
}
