using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Seguranca
{
    public class PermissaoUsuario : AbstractEntity
    {
        public bool Read { get; set; }
        public bool Add { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid PermissaoId { get; set; }

        public virtual Usuario Usuario{get;set;}
        public virtual Permissao Permissao { get; set; }
    }
}
