using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Account
{
    internal class FormaPagamentoMap : IEntityTypeConfiguration<FormaPagamento>
    {
        public void Configure(EntityTypeBuilder<FormaPagamento> builder)
        {
            builder.ToTable("FormaPagamento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);

            builder.Property(t => t.Nome).HasMaxLength(150);
            builder.Property(t => t.Codigo);

        }
    }
}
