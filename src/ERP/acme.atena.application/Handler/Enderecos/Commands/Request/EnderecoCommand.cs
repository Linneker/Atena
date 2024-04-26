using acme.atena.core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Commands.Request
{
    public class EnderecoCommand: CommandGuid
    {
        public EnderecoCommand()
        {
        }

        public EnderecoCommand(string cep, string pais, string estado, string cidade, string bairro, string rua)
        {
            Cep = cep;
            Pais = pais;
            Estado = estado;
            Cidade = cidade;
            Bairro = bairro;
            Rua = rua;
        }

        public string Cep { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
    }
}
