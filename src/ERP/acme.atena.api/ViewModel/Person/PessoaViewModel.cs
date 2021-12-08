using acme.atena.api.ViewModel.Account;
using System;
using System.Collections.Generic;

namespace acme.atena.api.ViewModel.Person
{
    public class PessoaViewModel : AbstractEntityViewModel
    {
        public PessoaViewModel()
        {
            Dividas = new HashSet<DividaViewModel>();
            Pagamentos = new HashSet<PagamentoViewModel>();
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public virtual ICollection<DividaViewModel> Dividas { get; set; }
    }
}
