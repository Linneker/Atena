using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Person
{
    public class FornecedorViewModel : AbstractEntityViewModel
    {
        public FornecedorViewModel()
        {
            Compras = new HashSet<CompraViewModel>();
        }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<CompraViewModel> Compras{ get; set; }
        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public virtual ICollection<DividaViewModel> Dividas { get; set; }

    }
}
