using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Util
{
    public class ParametroViewModel : AbstractEntityViewModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public bool ExibirTela { get; set; }
    }
}
