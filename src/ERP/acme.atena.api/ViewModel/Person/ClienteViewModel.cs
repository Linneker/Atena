using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Product;
using acme.atena.api.ViewModel.Security;
using acme.atena.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Person
{
    public class ClienteViewModel : PessoaViewModel
    {
        public ClienteViewModel()
        {
            Vendas = new HashSet<VendaViewModel>();
            EnderecoClientes = new HashSet<EnderecoClienteViewModel>();

        }

        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<VendaViewModel> Vendas { get; set; }
        public virtual ICollection<EnderecoClienteViewModel> EnderecoClientes { get; set; }

    }
}
