using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Security
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Login).HasMaxLength(500);
            builder.Property(t => t.Senha).HasMaxLength(400);
            builder.Property(t => t.Nome).HasMaxLength(400);

            builder.HasIndex(t => t.Login).IsUnique();

        }
    }
}
