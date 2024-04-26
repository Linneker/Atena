using acme.atena.core.Message;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Commands.Request
{
    public class EnderecoPessoaCommand: Command
    {
        public EnderecoPessoaCommand() { }
        public EnderecoPessoaCommand(string numero, string complemento, string latitude, string longitude, Guid? enderecoId)
        {
            Numero = numero;
            Complemento = complemento;
            Latitude = latitude;
            Longitude = longitude;
            EnderecoId = enderecoId;
        }

        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid? EnderecoId { get; set; }

        public virtual EnderecoCommand? Endereco { get; set; }

    }
}
