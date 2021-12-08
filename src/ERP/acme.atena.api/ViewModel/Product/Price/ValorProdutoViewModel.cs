using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Product.Price
{
    public class ValorProdutoViewModel : AbstractEntityViewModel
    {
        public decimal Valor { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid TipoValorProdutoId { get; set; }

        public ProdutoViewModel Produto { get; set; }
        public TipoValorProdutoViewModel TipoValorProduto { get; set; }
    }
}
