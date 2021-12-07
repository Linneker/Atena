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
    public class EnderecoUsuarioMap : IEntityTypeConfiguration<EnderecoUsuario>
    {
        public void Configure(EntityTypeBuilder<EnderecoUsuario> builder)
        {

            builder.ToTable("EnderecoUsuario");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Numero).HasMaxLength(10).IsRequired(true);
            builder.Property(t => t.Latitude).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Longitude).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Complemento).HasMaxLength(255).IsRequired(false);
            
            builder.HasOne(t => t.Usuario).WithMany(t=>t.EnderecoUsuarios).HasForeignKey(t=>t.UsuarioId);
            builder.HasOne(t => t.Endereco).WithMany(t => t.EnderecoUsuarios).HasForeignKey(t => t.EnderecoId);
        }
    }
}
