using acme.atena.api.ViewModel.Inventory;
using acme.atena.api.ViewModel.Util;
using System;
using System.Collections.Generic;

namespace acme.atena.api.ViewModel.Person
{
    public class EmpresaViewModel : PessoaViewModel
    {
        public bool IsFilial { get; set; }
        public Guid? EmpresaMatrizId { get; set; }
        public EmpresaViewModel EmpresaMatriz { get; set; }

        public string RazaoSocial { get; set; }
        public virtual ICollection<EstoqueViewModel> Estoque { get; set; }
        public virtual ICollection<EnderecoEmpresaViewModel> EnderecoEmpresas { get; set; } = new HashSet<EnderecoEmpresaViewModel>();

    }
}
