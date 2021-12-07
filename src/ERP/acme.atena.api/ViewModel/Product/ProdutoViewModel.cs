using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Product
{
    public class ProdutoViewModel : AbstractEntityViewModel
    {
        public ProdutoViewModel()
        {
            VendasProdutos = new HashSet<VendaProdutoViewModel>();
            ComprasProdutos = new HashSet<CompraProdutoViewModel>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long Codigo { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public virtual ICollection<VendaProdutoViewModel> VendasProdutos { get; set; }
        public virtual ICollection<CompraProdutoViewModel> ComprasProdutos { get; set; }


    }
}
