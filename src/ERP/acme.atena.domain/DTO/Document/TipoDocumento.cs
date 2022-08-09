using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Document
{
    public class TipoDocumento: AbstractEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Documento> Documentos{ get; set; } = new HashSet<Documento>();
    }
}
