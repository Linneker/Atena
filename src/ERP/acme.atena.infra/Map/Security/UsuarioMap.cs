using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Security
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
            builder.Property(t => t.TermoDeAceite);


            builder.Property(t => t.Nome).HasMaxLength(400);
            builder.Property(t => t.CpfCnpj).HasMaxLength(15);
            builder.Property(t => t.Celular).HasMaxLength(22);
            builder.Property(t => t.DataNascimento);
            builder.Property(t => t.Email).HasMaxLength(250);

            builder.HasIndex(t => t.CpfCnpj).IsUnique();
            builder.HasIndex(t => t.Email).IsUnique();
            builder.HasIndex(t => t.Login).IsUnique();

        }
    }
}
