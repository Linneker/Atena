using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Security
{
    public class PermissaoUsuarioViewModel : AbstractEntityViewModel
    {
        public bool Read { get; set; }
        public bool Add { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid PermissaoId { get; set; }

        public virtual UsuarioViewModel Usuario{get;set;}
        public virtual PermissaoViewModel Permissao { get; set; }
    }
}
