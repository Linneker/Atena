using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Util
{
    public abstract class EnderecoPessoa : AbstractEntity
    {
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid EnderecoId { get; set; }

        public virtual Endereco Endereco { get; set; }
    }
}
