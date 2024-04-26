using acme.atena.core.Message;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Commands.Request
{
    public class EnderecoFornecedorCommand: EnderecoPessoaCommand
    {
        public EnderecoFornecedorCommand()
        {

        }
        public EnderecoFornecedorCommand(bool matriz,string numero, string complemento, string latitude, string longitude, Guid? enderecoId)
            :base(numero, complemento, latitude, longitude, enderecoId)
        {
            Matriz = matriz;
        }

        public bool Matriz { get; set; }
    }
}
