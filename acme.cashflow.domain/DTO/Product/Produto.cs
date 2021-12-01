using acme.cashflow.domain.DTO.Product.Price;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Product
{
    public class Produto : AbstractEntity
    {
        public Produto()
        {
            VendasProdutos = new HashSet<VendaProduto>();
            ComprasProdutos = new HashSet<CompraProduto>();
            ValorProdutos = new HashSet<ValorProduto>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long Codigo { get; set; }
        public int Quantidade { get; set; }
        public Guid TipoProdutoId { get; set; }
        public TipoProduto TipoProduto { get; set; }
        public virtual ICollection<ValorProduto> ValorProdutos { get; set; }
        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }
        public virtual ICollection<CompraProduto> ComprasProdutos { get; set; }


    }
}
