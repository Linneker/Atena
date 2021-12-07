using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.infra.Map.Person
{
    public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {

            builder.ToTable("Fornecedor");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Nome).HasMaxLength(400);
            builder.Property(t => t.CpfCnpj).HasMaxLength(15);
            builder.Property(t => t.Celular).HasMaxLength(22);
            builder.Property(t => t.DataNascimento);
            builder.Property(t => t.Email).HasMaxLength(250);
            builder.Property(t => t.NomeFantasia).HasMaxLength(250);
            builder.Property(t => t.InscricaoMunicipal).HasMaxLength(50);

            builder.HasIndex(t => t.CpfCnpj).IsUnique();
            builder.HasIndex(t => t.Email).IsUnique();
        }
    }
}
