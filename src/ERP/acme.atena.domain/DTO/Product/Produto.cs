using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.DTO.Product.Sequence;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Product
{
    public class Produto : AbstractEntity
    {
        public Produto()
        {
            VendasProdutos = new HashSet<VendaProduto>();
            ComprasProdutos = new HashSet<CompraProduto>();
            ValorProdutos = new HashSet<ValorProduto>();
            EntradaProdutoEstoques = new HashSet<EntradaProdutoEstoque>();
            SaidaProdutoEstoques = new HashSet<SaidaProdutoEstoque>();
            EstoqueProdutos = new HashSet<EstoqueProduto>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long Codigo { get; set; }
        public DateTime? Validade { get; set; }
        public Guid TipoProdutoId { get; set; }
        public TipoProduto TipoProduto { get; set; }

        public virtual CodigoProduto CodigoProduto { get;set;}
        public virtual ICollection<ValorProduto> ValorProdutos { get; set; }
        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }
        public virtual ICollection<CompraProduto> ComprasProdutos { get; set; }
        public virtual ICollection<EntradaProdutoEstoque> EntradaProdutoEstoques { get; set; }
        public virtual ICollection<SaidaProdutoEstoque> SaidaProdutoEstoques { get; set; }
        public virtual ICollection<EstoqueProduto> EstoqueProdutos { get; set; }
        public virtual ICollection<DevolucaoCompra> DevolucaoCompras { get; set; } = new HashSet<DevolucaoCompra>();
        public virtual ICollection<DevolucaoVenda> DevolucoesVenda { get; set; } = new HashSet<DevolucaoVenda>();

        public bool Atualizar(Produto produto)
        {
            bool temProdutoAtualizar = false;
            if (Nome != produto.Nome)
            {
                Nome = produto.Nome;
                temProdutoAtualizar = true;
            }
            if(Validade != produto.Validade)
            {
                Validade = produto.Validade;
                temProdutoAtualizar = true;
            }
            if(TipoProdutoId != produto.TipoProdutoId)
            {
                TipoProdutoId = produto.TipoProdutoId;
                temProdutoAtualizar = true;
            }
            if(Descricao != produto.Descricao)
            {
                Descricao = produto.Descricao;
                temProdutoAtualizar = true;
            }
            return temProdutoAtualizar;
        }
    }
}
