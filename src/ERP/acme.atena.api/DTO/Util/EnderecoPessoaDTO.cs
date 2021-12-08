using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.DTO.Util
{
    public abstract class EnderecoPessoaDTO : AbstractEntity
    {
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid EnderecoId { get; set; }

        public virtual EnderecoDTO Endereco { get; set; }
    }
}
