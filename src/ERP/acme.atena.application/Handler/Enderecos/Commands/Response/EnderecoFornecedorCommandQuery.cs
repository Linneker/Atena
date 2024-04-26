using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Commands.Response
{
    public class EnderecoFornecedorCommandQuery: EnderecoPessoaCommandQuery
    {
        public bool Matriz { get; set; }
    }
}
