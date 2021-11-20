using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace acme.cashflow.domain.DTO.Product
{
    public class Venda : AbstractEntity
    {
        public Venda()
        {
            VendasProdutos = new HashSet<VendaProduto>();
        }

        public Venda(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId) : this()
        {
            DataVenda = dataVenda;
            ValorTotal = valorTotal;
            UsuarioId = usuarioId;
            PessoaId = pessoaId;
        }

        public Venda(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId, ICollection<VendaProduto> vendasProdutos)
        {
            DataVenda = dataVenda;
            ValorTotal = valorTotal;
            UsuarioId = usuarioId;
            PessoaId = pessoaId;
            VendasProdutos = vendasProdutos;
        }

        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }

        public Guid UsuarioId { get; set; }
        public Guid PessoaId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Competencia Competencia { get; set; }
        public virtual ICollection<VendaProduto> VendasProdutos { get; set; }         


    }
}
