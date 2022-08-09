using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map
{
    public class BaseMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AbstractEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

            builder.HasKey(t => t.Id);

            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);
            builder.Property(t => t.UsuarioCriacao);
            builder.Property(t => t.UsuarioModificacao);
        }
    }
}
