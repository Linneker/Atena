using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Security
{
    public class PermissaoUsuarioViewModel : AbstractEntityViewModel
    {
        public string Acesso { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid PermissaoId { get; set; }
        public Guid TelaId { get; set; }

        public Tela Tela { get; set; }
        public virtual UsuarioViewModel Usuario{get;set;}
        public virtual PermissaoViewModel Permissao { get; set; }
    }
}
