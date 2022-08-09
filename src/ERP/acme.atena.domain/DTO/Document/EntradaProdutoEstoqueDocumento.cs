using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Document
{
    public class EntradaProdutoEstoqueDocumento: AbstractEntity
    {
        public Guid EntradaProdutoEstoqueId { get; set; }
        public Guid DocumentoId { get; set; }

        public virtual EntradaProdutoEstoque EntradaProdutoEstoque { get; set; }
        public virtual Documento Documento { get; set; }

    }
}
