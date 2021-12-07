using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Person
{
    public class EnderecoFornecedorMap : IEntityTypeConfiguration<EnderecoFornecedor>
    {
        public void Configure(EntityTypeBuilder<EnderecoFornecedor> builder)
        {

            builder.ToTable("EnderecoFornecedor");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Numero).HasMaxLength(10).IsRequired(true);
            builder.Property(t => t.Latitude).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Longitude).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Complemento).HasMaxLength(255).IsRequired(false);

            builder.HasOne(t => t.Fornecedor).WithMany(t => t.EnderecoFornecedores).HasForeignKey(t => t.FornecedorId);
            builder.HasOne(t => t.Endereco).WithMany(t => t.EnderecoFornecedores).HasForeignKey(t => t.EnderecoId);
        }
    }
}
