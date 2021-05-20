using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.infra.Map.Account
{
    public class ReceitaMap : IEntityTypeConfiguration<Receita>
    {
        public void Configure(EntityTypeBuilder<Receita> builder)
        {
            builder.ToTable("Receita");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Descricao);
            builder.Property(t => t.Nome).HasMaxLength(255);
            builder.Property(t => t.ReceitaFixa).HasDefaultValue(false);
            builder.Property(t => t.Valor);

            builder.HasOne(t => t.Competencia).WithMany(t => t.Receitas).HasForeignKey(t => t.CompetenciaId);

        }
    }
}
