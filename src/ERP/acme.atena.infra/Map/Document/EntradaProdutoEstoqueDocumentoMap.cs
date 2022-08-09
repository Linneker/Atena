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
    public class EntradaProdutoEstoqueDocumentoMap: BaseMap<EntradaProdutoEstoqueDocumento>
    {
        public override void Configure(EntityTypeBuilder<EntradaProdutoEstoqueDocumento> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(EntradaProdutoEstoqueDocumento));

            builder.HasOne(t => t.EntradaProdutoEstoque).WithMany(t => t.EntradaProdutoEstoquesDocumentos).HasForeignKey(t => t.EntradaProdutoEstoqueId);
            builder.HasOne(t => t.Documento).WithMany(t => t.EntradaProdutoEstoquesDocumentos).HasForeignKey(t => t.DocumentoId);

        }
    }
}
