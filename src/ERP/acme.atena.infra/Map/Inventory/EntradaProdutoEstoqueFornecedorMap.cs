using acme.atena.domain.DTO.Inventory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.infra.Map.Inventory
{
    public class EntradaProdutoEstoqueFornecedorMap: BaseMap<EntradaProdutoEstoqueFornecedor>
    {
        public override void Configure(EntityTypeBuilder<EntradaProdutoEstoqueFornecedor> builder)
        {
            base.Configure(builder);

            builder.HasOne(T => T.EntradaProdutoEstoque).WithMany(t => t.EntradaProdutoEstoqueFornecedor).HasForeignKey(t => t.EntradaProdutoEstoqueId);
            builder.HasOne(T => T.Fornecedor).WithMany(t => t.EntradaProdutoEstoqueFornecedor).HasForeignKey(t => t.FornecedorId);

        }
    }
}
