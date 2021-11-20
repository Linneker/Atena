using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Util
{
    public class CompetenciaMap : IEntityTypeConfiguration<Competencia>
    {
        public void Configure(EntityTypeBuilder<Competencia> builder)
        {
            builder.ToTable("Competencia");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Ano);
            builder.Property(t => t.Mes);
        }
    }
}
