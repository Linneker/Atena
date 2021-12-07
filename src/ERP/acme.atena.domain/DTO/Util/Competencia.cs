using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Util
{
    public class Competencia : AbstractEntity
    {
        public Competencia()
        {
            Despesas = new HashSet<Despesa>();
            FluxosDeCasas = new HashSet<FluxoDeCaixa>();
            Receitas = new HashSet<Receita>();

            Dividas = new HashSet<Divida>();
            Pagamentos = new HashSet<Pagamento>();
            Compras = new HashSet<Compra>();
            Vendas = new HashSet<Venda>();
            VendasProdutos =   new HashSet<VendaProduto>();

        }
        public Competencia(int ano, int mes) : this()
        {
            Ano = ano;
            Mes = mes;
        }

        public int Ano { get; set; }
        public int Mes { get; set; }

        public virtual ICollection<Despesa> Despesas { get; set; }
        public virtual ICollection<FluxoDeCaixa> FluxosDeCasas { get; set; }
        public virtual ICollection<Receita> Receitas { get; set; }

        public virtual ICollection<Divida> Dividas { get; set; }
        public virtual ICollection<Pagamento> Pagamentos{ get; set; }
        public virtual ICollection<Compra> Compras { get; set; }
        public virtual ICollection<Venda> Vendas { get; set; }
        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }
        public virtual ICollection<CompraProduto> ComprasProdutos { get; set; } = new HashSet<CompraProduto>();
        public virtual ICollection<DevolucaoCompra> DevolucaoCompras{ get; set; } = new HashSet<DevolucaoCompra>();
        public virtual ICollection<DevolucaoVenda> DevolucoesVenda { get; set; } = new HashSet<DevolucaoVenda>();
        public virtual ICollection<EntradaProdutoEstoque> EntradaProdutoEstoques { get; set; } = new HashSet<EntradaProdutoEstoque>();
        public virtual ICollection<SaidaProdutoEstoque> SaidaProdutoEstoques { get; set; } = new HashSet<SaidaProdutoEstoque>();
        public virtual ICollection<PagamentoVenda> PagamentosVendas { get; set; } = new HashSet<PagamentoVenda>();
        
    }
}
