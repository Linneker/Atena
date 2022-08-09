using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public class EntradaProdutoEstoqueFornecedor: AbstractEntity
    {
        public Guid EntradaProdutoEstoqueId { get; set; }
        public Guid FornecedorId { get; set; }

        public virtual EntradaProdutoEstoque EntradaProdutoEstoque { get; set; }
        public virtual Fornecedor Fornecedor{ get; set; }

    }
}
