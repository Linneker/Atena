using acme.atena.api.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Inventory
{
    public class SaidaProdutoEstoqueViewModel : AbstractEntityCompetenciaViewModel
    {
        public SaidaProdutoEstoqueViewModel():base()
        {
            DataSaida = DateTime.Now;
        }
        public SaidaProdutoEstoqueViewModel(Guid produtoId, Guid estoqueId, int quantidade) : this()
        {
            ProdutoId = produtoId;
            EstoqueId = estoqueId;
            Quantidade = quantidade;
        }

        public SaidaProdutoEstoqueViewModel(Guid produtoId, Guid estoqueId, int quantidade, DateTime dataSaida) : this(produtoId, estoqueId, quantidade)
        {
            DataSaida = dataSaida;
        }

        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataSaida { get; set; }

        public virtual EstoqueViewModel Estoque { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }

    }
}
