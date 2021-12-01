using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.DTO.Product.Price
{
    public class ValorProduto : AbstractEntity
    {
        public decimal Valor { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid TipoValorProdutoId { get; set; }

        public Produto Produto { get; set; }
        public TipoValorProduto TipoValorProduto { get; set; }
    }
}
