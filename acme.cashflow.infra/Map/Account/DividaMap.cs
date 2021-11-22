using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.infra.Map.Account
{
    public class DividaMap : IEntityTypeConfiguration<Divida>
    {
        public void Configure(EntityTypeBuilder<Divida> builder)
        {
            builder.ToTable("Divida");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.HasOne(t => t.Usuario).WithMany(t => t.Dividas).HasForeignKey(t => t.UsuarioId);
            builder.HasOne(t => t.Pessoa).WithMany(t => t.Dividas).HasForeignKey(t => t.PessoaId);
            builder.HasOne(t => t.Fornecedor).WithMany(t => t.Dividas).HasForeignKey(t => t.FornecedorId);
            builder.HasOne(t => t.Competencia).WithMany(t => t.Dividas).HasForeignKey(t => t.CompetenciaId);
        }
    }
}
