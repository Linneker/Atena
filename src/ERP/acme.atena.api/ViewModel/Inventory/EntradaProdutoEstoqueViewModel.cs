using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Inventory
{
    public class EntradaProdutoEstoqueViewModel : AbstractEntityCompetenciaViewModel
    {
        public EntradaProdutoEstoqueViewModel() : base()
        {
            DataEntrada = DateTime.Now;
        }
        public EntradaProdutoEstoqueViewModel(Guid produtoId, Guid estoqueId) : this()
        {
            ProdutoId = produtoId;
            EstoqueId = estoqueId;
        }

        public EntradaProdutoEstoqueViewModel(Guid produtoId, Guid estoqueId, DateTime dataEntrada) : this(produtoId, estoqueId)
        {
            DataEntrada = dataEntrada;
        }

        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public DateTime DataEntrada { get; set; }

        public virtual EstoqueViewModel Estoque { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }

    }
}
