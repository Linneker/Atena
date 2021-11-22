using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace acme.cashflow.api.ViewModel.Product
{
    public class VendaViewModel : AbstractEntityViewModel
    {
        public VendaViewModel()
        {
            VendasProdutos = new HashSet<VendaProdutoViewModel>();
        }

        public VendaViewModel(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId) : this()
        {
            DataVenda = dataVenda;
            ValorTotal = valorTotal;
            UsuarioId = usuarioId;
            PessoaId = pessoaId;
        }

        public VendaViewModel(DateTime dataVenda, decimal valorTotal, Guid usuarioId, Guid pessoaId, ICollection<VendaProdutoViewModel> vendasProdutos)
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

        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual PessoaViewModel Pessoa { get; set; }
        public virtual CompetenciaViewModel Competencia { get; set; }
        public virtual ICollection<VendaProdutoViewModel> VendasProdutos { get; set; }         


    }
}
