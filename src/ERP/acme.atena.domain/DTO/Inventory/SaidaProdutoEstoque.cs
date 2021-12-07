using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public class SaidaProdutoEstoque : AbstractEntityCompetencia
    {
        public SaidaProdutoEstoque():base()
        {
            DataSaida = DateTime.Now;
        }
        public SaidaProdutoEstoque(Guid produtoId, Guid estoqueId, int quantidade) : this()
        {
            ProdutoId = produtoId;
            EstoqueId = estoqueId;
            Quantidade = quantidade;
        }

        public SaidaProdutoEstoque(Guid produtoId, Guid estoqueId, int quantidade, DateTime dataSaida) : this(produtoId, estoqueId, quantidade)
        {
            DataSaida = dataSaida;
        }

        public Guid ProdutoId { get; set; }
        public Guid EstoqueId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataSaida { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Produto Produto { get; set; }

    }
}
