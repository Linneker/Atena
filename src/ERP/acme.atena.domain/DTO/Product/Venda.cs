using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace acme.atena.domain.DTO.Product
{
    public class Venda : AbstractEntityCompetencia
    {
        public Venda()
        {
            VendasProdutos = new HashSet<VendaProduto>();
            PagamentosVendas = new HashSet<PagamentoVenda>();
        }

        public Venda(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId) : this()
        {
            DataVenda = dataVenda;
            ValorTotal = valorTotal;
            UsuarioId = usuarioId;
            ClienteId = pessoaId;
        }

        public Venda(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId, ICollection<VendaProduto> vendasProdutos)
        {
            DataVenda = dataVenda;
            ValorTotal = valorTotal;
            UsuarioId = usuarioId;
            ClienteId = pessoaId;
            VendasProdutos = vendasProdutos;
        }

        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }

        public Guid UsuarioId { get; set; }
        public Guid? ClienteId { get; set; }
        public Guid? EmpresaId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }
        public virtual ICollection<PagamentoVenda> PagamentosVendas { get; set; }


    }
}
