using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class FluxoDeCaixaDespesaViewModel : AbstractEntityViewModel
    {
        public Guid FluxoDeCaixaId { get; set; }
        public Guid DespesaId { get; set; }

        public virtual FluxoDeCaixaViewModel FluxoDeCaixa { get; set; }
        public virtual DespesaViewModel Despesa { get; set; }
    }
}
