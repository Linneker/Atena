using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Security
{
    public class PermissaoUsuarioMap : IEntityTypeConfiguration<PermissaoUsuario>
    {
        public void Configure(EntityTypeBuilder<PermissaoUsuario> builder)
        {
            builder.ToTable("PermissaoUsuario");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Read).HasDefaultValue(true);
            builder.Property(t => t.Add).HasDefaultValue(false);
            builder.Property(t => t.Delete).HasDefaultValue(false);
            builder.Property(t => t.Update).HasDefaultValue(false);

            builder.HasOne(t => t.Usuario).WithMany(t => t.PermissaoUsuarios).HasForeignKey(t => t.UsuarioId);
            builder.HasOne(t => t.Permissao).WithMany(t => t.PermissaoUsuarios).HasForeignKey(t => t.UsuarioId);

        }
    }
}
