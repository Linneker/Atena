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
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long Codigo { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }
        public virtual ICollection<CompraProduto> ComprasProdutos { get; set; }


    }
}
