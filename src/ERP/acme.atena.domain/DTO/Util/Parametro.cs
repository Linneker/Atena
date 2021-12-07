using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Util
{
    public class Parametro : AbstractEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public bool ExibirTela { get; set; }
    }
}
