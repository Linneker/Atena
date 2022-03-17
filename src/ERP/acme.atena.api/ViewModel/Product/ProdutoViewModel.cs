using acme.atena.api.ViewModel.Inventory;
using acme.atena.api.ViewModel.Product.Price;
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
        public DateTime? Validade { get; set; }
        public Guid TipoProdutoId { get; set; }
        public TipoProdutoViewModel TipoProduto { get; set; }
        public virtual ICollection<ValorProdutoViewModel> ValorProdutos { get; set; }
        public virtual ICollection<VendaProdutoViewModel> VendasProdutos { get; set; }
        public virtual ICollection<CompraProdutoViewModel> ComprasProdutos { get; set; }
        public virtual ICollection<EntradaProdutoEstoqueViewModel> EntradaProdutoEstoques { get; set; }
        public virtual ICollection<SaidaProdutoEstoqueViewModel> SaidaProdutoEstoques { get; set; }
        public virtual ICollection<EstoqueProdutoViewModel> EstoqueProdutos { get; set; }
        public virtual ICollection<DevolucaoCompraViewModel> DevolucaoCompras { get; set; } = new HashSet<DevolucaoCompraViewModel>();
        public virtual ICollection<DevolucaoVendaViewModel> DevolucoesVenda { get; set; } = new HashSet<DevolucaoVendaViewModel>();
    }
}
