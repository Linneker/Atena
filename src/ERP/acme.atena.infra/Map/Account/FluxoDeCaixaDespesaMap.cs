using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Account
{
    public class FluxoDeCaixaDespesaMap : IEntityTypeConfiguration<FluxoDeCaixaDespesa>
    {
        public void Configure(EntityTypeBuilder<FluxoDeCaixaDespesa> builder)
        {
            builder.ToTable("FluxoDeCaixaDespesa");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Despesa).WithMany(t => t.FluxoDeCaixasDespesas).HasForeignKey(t => t.DespesaId);
            builder.HasOne(t => t.FluxoDeCaixa).WithMany(t => t.FluxoDeCaixasDespesas).HasForeignKey(t => t.FluxoDeCaixaId);
        }
    }
}
