using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Util
{
    public abstract class EnderecoPessoaViewModel : AbstractEntityViewModel
    {
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Guid EnderecoId { get; set; }

        public virtual EnderecoViewModel Endereco { get; set; }
    }
}
