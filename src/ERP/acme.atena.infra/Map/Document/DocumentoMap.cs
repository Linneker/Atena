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
    public class DocumentoMap: BaseMap<Documento>
    {
        public override void Configure(EntityTypeBuilder<Documento> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(Documento));

            builder.Property(t => t.Token).HasMaxLength(200);
            builder.Property(t => t.Hash).HasMaxLength(500);
            builder.Property(t => t.Container).HasMaxLength(200);

            builder.HasOne(t => t.TipoDocumento).WithMany(t => t.Documentos).HasForeignKey(T => T.TipoDocumentoId);
        }
    }
}
