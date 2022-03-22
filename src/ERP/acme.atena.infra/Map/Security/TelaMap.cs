using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Security
{
    public class TelaMap : IEntityTypeConfiguration<Tela>
    {
        public void Configure(EntityTypeBuilder<Tela> builder)
        {
            builder.ToTable("Tela");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
            builder.Property(t => t.Url).HasMaxLength(255).IsRequired(false); 
            builder.Property(t => t.Variant).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Class).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.Icon).HasMaxLength(255).IsRequired(false);
            builder.Property(t => t.IsPrincipal).HasDefaultValue(false);
            builder.Property(t => t.Title).HasMaxLength(255);
            builder.Property(t => t.TelaSistemaFilhaId);


            builder.HasOne(t => t.TelaSistemaFilha).WithMany(t => t.TelasSistemasFilhas).HasForeignKey(t => t.TelaSistemaFilhaId);

        }
    }
}
