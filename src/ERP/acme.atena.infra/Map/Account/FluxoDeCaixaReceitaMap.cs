using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Account
{
    public class FluxoDeCaixaReceitaMap : IEntityTypeConfiguration<FluxoDeCaixaReceita>
    {
        public void Configure(EntityTypeBuilder<FluxoDeCaixaReceita> builder)
        {
            builder.ToTable("FluxoDeCaixaReceita");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Receita).WithMany(t => t.FluxoDeCaixasReceitas).HasForeignKey(t => t.ReceitaId);
            builder.HasOne(t => t.FluxoDeCaixa).WithMany(t => t.FluxoDeCaixasReceitas).HasForeignKey(t => t.FluxoDeCaixaId);
        }
    }
}
