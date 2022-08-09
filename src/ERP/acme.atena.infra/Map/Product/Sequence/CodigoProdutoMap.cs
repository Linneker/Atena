using acme.atena.domain.DTO.Product.Sequence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Product.Sequence
{
    public class CodigoProdutoMap : IEntityTypeConfiguration<CodigoProduto>
    {
        public void Configure(EntityTypeBuilder<CodigoProduto> builder)
        {
            builder.ToTable("Sequence_Codigo_Produto");
            builder.HasKey(x => x.Id);

            builder.Property(t => t.DataCriacao);
        }
    }
}
