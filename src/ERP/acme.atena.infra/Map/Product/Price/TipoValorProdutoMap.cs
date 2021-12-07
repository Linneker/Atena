using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product.Price;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Product.Price
{
    internal class TipoValorProdutoMap : IEntityTypeConfiguration<TipoValorProduto>
    {
        public void Configure(EntityTypeBuilder<TipoValorProduto> builder)
        {
            builder.ToTable("TipoValorProduto");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.DataCriacao);
            builder.Property(t => t.DataModificacao).IsRequired(false);
            builder.Property(t => t.DataInativacao).IsRequired(false);
            builder.Property(t => t.Status).HasDefaultValue(EnumStatus.Ativo);


        }
    }
}
