using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.ViewModel.Security
{
    public class UsuarioViewModel : AbstractEntityViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
    }
}
