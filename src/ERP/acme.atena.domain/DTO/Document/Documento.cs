using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Document
{
    public class Documento: AbstractEntity
    {
        public string Token { get; set; }
        public string Container { get; set; }
        public string Hash { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }

        public virtual ICollection<EntradaProdutoEstoqueDocumento> EntradaProdutoEstoquesDocumentos { get; set; } = new HashSet<EntradaProdutoEstoqueDocumento>();

    }
}
