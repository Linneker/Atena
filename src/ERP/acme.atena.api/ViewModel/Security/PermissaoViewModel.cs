using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Security
{
    public class PermissaoViewModel : AbstractEntityViewModel
    {
        public PermissaoViewModel()
        {
            PermissaoUsuarios = new HashSet<PermissaoUsuarioViewModel>();
        }
        public string Nome { get; set; }
        public int Nivel { get; set; }

        public virtual ICollection<PermissaoUsuarioViewModel> PermissaoUsuarios { get; set; }
    }
}
