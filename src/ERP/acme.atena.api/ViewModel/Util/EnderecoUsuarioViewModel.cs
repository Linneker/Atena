using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public class EnderecoUsuarioViewModel : EnderecoPessoaViewModel
    {
        public Guid UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
