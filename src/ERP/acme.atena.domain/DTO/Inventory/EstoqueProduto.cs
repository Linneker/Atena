using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public class EstoqueProduto : AbstractEntity
    {
        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public long QuantidadeMaxima { get; set; }
        public long QuantidadeMinima { get; set; }
        public long QuantidadeProduto { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
