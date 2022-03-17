using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Seguranca
{
    public class PermissaoUsuario : AbstractEntity
    {
        public string Acesso { get; set; }

        public Guid TelaId { get; set; }

        public Tela Tela { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid PermissaoId { get; set; }

        public virtual Usuario Usuario{get;set;}
        public virtual Permissao Permissao { get; set; }
    }
}
