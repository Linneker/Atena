using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public class EnderecoViewModel: AbstractEntityViewModel
    {
        public EnderecoViewModel()
        {
            EnderecoClientes = new HashSet<EnderecoClienteViewModel>();
            EnderecoFornecedores = new HashSet<EnderecoFornecedorViewModel>();
            EnderecoUsuarios = new HashSet<EnderecoUsuarioViewModel>();

        }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }

        public virtual ICollection<EnderecoClienteViewModel> EnderecoClientes { get; set; }
        public virtual ICollection<EnderecoUsuarioViewModel> EnderecoUsuarios { get; set; }
        public virtual ICollection<EnderecoFornecedorViewModel> EnderecoFornecedores { get; set; }
        public virtual ICollection<EnderecoEmpresaViewModel> EnderecoEmpresas { get; set; } = new HashSet<EnderecoEmpresaViewModel>();

    }
}
