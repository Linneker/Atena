using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Seguranca
{
    public class Permissao : AbstractEntity
    {
        public Permissao()
        {
            PermissaoUsuarios = new HashSet<PermissaoUsuario>();
        }
        public string Nome { get; set; }
        public int Nivel { get; set; }

        public virtual ICollection<PermissaoUsuario> PermissaoUsuarios { get; set; }
    }
}
