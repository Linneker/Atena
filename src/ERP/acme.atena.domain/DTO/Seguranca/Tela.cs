using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Seguranca
{
    public class Tela: AbstractEntity
    {
        public string Name { get; set; }
        public string Url { get; set;}
        public string Icon { get; set;}
        public bool Title { get; set; } = false;
        public bool IsPrincipal { get; set; }
        public string Class { get; set; }
        public string Variant { get; set; }

        public Guid TelaSistemaFilhaId { get; set; }

        public Tela TelaSistemaFilha { get; set; }

        public ICollection<Tela> TelasSistemasFilhas { get; set; } = new HashSet<Tela>();
        public ICollection<PermissaoUsuario> PermissaoUsuarios { get; set; } = new HashSet<PermissaoUsuario>();

    }
}
