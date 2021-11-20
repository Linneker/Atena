using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.ViewModel.Account
{
    public class FluxoDeCaixaReceitaViewModel : AbstractEntityViewModel
    {
        public Guid FluxoDeCaixaId { get; set; }
        public Guid  ReceitaId { get; set; }

        public virtual FluxoDeCaixaViewModel FluxoDeCaixa { get; set; }

        public virtual ReceitaViewModel Receita { get; set; }

    }
}
