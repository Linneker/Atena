using acme.atena.domain.DTO.Document;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Document
{
    public class TipoDocumentoMap: BaseMap<TipoDocumento>
    {
        public override void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(TipoDocumento));

            builder.Property(t => t.Nome).HasMaxLength(200);
            builder.Property(t => t.Descricao).HasMaxLength(200);

        }
    }
}
