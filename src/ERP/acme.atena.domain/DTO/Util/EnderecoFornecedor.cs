using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Util
{
    public class EnderecoFornecedor : EnderecoPessoa
    {
        public Guid FornecedorId { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }
    }
}
