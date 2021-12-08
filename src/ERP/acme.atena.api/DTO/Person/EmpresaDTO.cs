using acme.atena.api.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.DTO.Person
{
    public class EmpresaDTO : PessoaDTO
    {
        public bool IsFilial { get; set; }
        public Guid? EmpresaMatrizId { get; set; }
        public EmpresaDTO EmpresaMatriz { get; set; }

        public string RazaoSocial { get; set; }
        public virtual ICollection<EnderecoEmpresaDTO> EnderecoEmpresas { get; set; } = new HashSet<EnderecoEmpresaDTO>();
    }
}
