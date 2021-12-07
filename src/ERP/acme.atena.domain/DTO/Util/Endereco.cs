using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Util
{
    public class Endereco: AbstractEntity
    {
        public Endereco()
        {
            EnderecoClientes = new HashSet<EnderecoCliente>();
            EnderecoFornecedores = new HashSet<EnderecoFornecedor>();
            EnderecoUsuarios = new HashSet<EnderecoUsuario>();

        }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }

        public virtual ICollection<EnderecoCliente> EnderecoClientes { get; set; }
        public virtual ICollection<EnderecoUsuario> EnderecoUsuarios { get; set; }
        public virtual ICollection<EnderecoFornecedor> EnderecoFornecedores { get; set; }
        public virtual ICollection<EnderecoEmpresa> EnderecoEmpresas { get; set; } = new HashSet<EnderecoEmpresa>();

    }
}
