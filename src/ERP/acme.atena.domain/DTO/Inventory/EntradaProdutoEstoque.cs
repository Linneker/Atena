using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public class EntradaProdutoEstoque : AbstractEntityCompetencia
    {
        public EntradaProdutoEstoque() : base()
        {
            DataEntrada = DateTime.Now;
        }
        public EntradaProdutoEstoque(Guid produtoId, Guid estoqueId) : this()
        {
            ProdutoId = produtoId;
            EstoqueId = estoqueId;
        }

        public EntradaProdutoEstoque(Guid produtoId, Guid estoqueId, DateTime dataEntrada) : this(produtoId, estoqueId)
        {
            DataEntrada = dataEntrada;
        }

        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public DateTime DataEntrada { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Produto Produto { get; set; }

    }
}
