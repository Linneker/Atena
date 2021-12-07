using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Util
{
    public class EnderecoEmpresa : EnderecoPessoa
    {
        public Guid EmpresaId { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
