using acme.atena.api.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.DTO.Util
{
    public class EnderecoDTO: AbstractEntity
    {
        public EnderecoDTO()
        {

        }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }


    }
}
