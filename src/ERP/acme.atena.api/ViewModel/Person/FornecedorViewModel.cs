using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Product;
using acme.atena.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Person
{
    public class FornecedorViewModel : PessoaViewModel
    {
        public FornecedorViewModel()
        {
            Compras = new HashSet<CompraViewModel>();
            EnderecoFornecedores = new HashSet<EnderecoFornecedorViewModel>();
        }

        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<CompraViewModel> Compras { get; set; }
        public virtual ICollection<EnderecoFornecedorViewModel> EnderecoFornecedores { get; set; }

    }
}
