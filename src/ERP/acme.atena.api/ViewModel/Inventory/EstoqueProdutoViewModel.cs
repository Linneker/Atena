using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Inventory
{
    public class EstoqueProdutoViewModel : AbstractEntityViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public long QuantidadeMaxima { get; set; }
        public long QuantidadeMinima { get; set; }
        public long QuantidadeProduto { get; set; }

        public virtual EstoqueViewModel Estoque { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }
    }
}
