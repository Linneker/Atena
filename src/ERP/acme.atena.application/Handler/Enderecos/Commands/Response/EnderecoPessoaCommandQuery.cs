using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Commands.Response
{
    public class EnderecoPessoaCommandQuery
    {
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid? EnderecoId { get; set; }

        public virtual EnderecoCommandQuery? Endereco { get; set; }

    }
}
