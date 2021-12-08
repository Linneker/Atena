using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Inventory;
using acme.atena.api.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public class CompetenciaViewModel : AbstractEntityViewModel
    {
        public CompetenciaViewModel()
        {
            Despesas = new HashSet<DespesaViewModel>();
            FluxosDeCasas = new HashSet<FluxoDeCaixaViewModel>();
            Receitas = new HashSet<ReceitaViewModel>();
        }

        public int Ano { get; set; }
        public int Mes { get; set; }

        public virtual ICollection<DespesaViewModel> Despesas { get; set; }
        public virtual ICollection<FluxoDeCaixaViewModel> FluxosDeCasas { get; set; }
        public virtual ICollection<ReceitaViewModel> Receitas { get; set; }


        public virtual ICollection<DividaViewModel> Dividas { get; set; }
        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public virtual ICollection<CompraViewModel> Compras { get; set; }
        public virtual ICollection<VendaViewModel> Vendas { get; set; }
        public virtual ICollection<VendaProdutoViewModel> VendasProdutos { get; set; }
        public virtual ICollection<CompraProdutoViewModel> ComprasProdutos { get; set; } = new HashSet<CompraProdutoViewModel>();
        public virtual ICollection<DevolucaoCompraViewModel> DevolucaoCompras { get; set; } = new HashSet<DevolucaoCompraViewModel>();
        public virtual ICollection<DevolucaoVendaViewModel> DevolucoesVenda { get; set; } = new HashSet<DevolucaoVendaViewModel>();
        public virtual ICollection<EntradaProdutoEstoqueViewModel> EntradaProdutoEstoques { get; set; } = new HashSet<EntradaProdutoEstoqueViewModel>();
        public virtual ICollection<SaidaProdutoEstoqueViewModel> SaidaProdutoEstoques { get; set; } = new HashSet<SaidaProdutoEstoqueViewModel>();
        public virtual ICollection<PagamentoVendaViewModel> PagamentosVendas { get; set; } = new HashSet<PagamentoVendaViewModel>();

    }
}
