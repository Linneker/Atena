using acme.cashflow.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.DTO.Inventory
{
    public class SaidaProdutoEstoque : AbstractEntity
    {
        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public int Quantidade { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Produto Produto { get; set; }

    }
}
