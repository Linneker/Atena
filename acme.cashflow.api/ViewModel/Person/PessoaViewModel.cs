using acme.cashflow.api.ViewModel.Account;
using acme.cashflow.api.ViewModel.Product;
using acme.cashflow.api.ViewModel.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Person
{
    public class PessoaViewModel : AbstractEntityViewModel
    {
        public PessoaViewModel()
        {
            Usuarios = new HashSet<UsuarioViewModel>();
            Vendas = new HashSet<VendaViewModel>();
            Dividas = new HashSet<DividaViewModel>();
            Pagamentos = new HashSet<PagamentoViewModel>();
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<UsuarioViewModel> Usuarios { get; set; }
        public virtual ICollection<PagamentoViewModel> Pagamentos{ get; set; }
        public virtual ICollection<VendaViewModel> Vendas{ get; set; }
        public virtual ICollection<DividaViewModel> Dividas { get; set; }

    }
}
