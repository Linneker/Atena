using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Person
{
    public class Empresa : Pessoa
    {
        public bool IsFilial { get; set; }
        public Guid? EmpresaMatrizId { get; set; }
        public Empresa EmpresaMatriz { get; set; }

        public string RazaoSocial { get; set; }
        public virtual ICollection<Estoque> Estoque { get; set; }
        public virtual ICollection<EnderecoEmpresa> EnderecoEmpresas { get; set; } = new HashSet<EnderecoEmpresa>();
    }
}
