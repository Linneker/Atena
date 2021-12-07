using acme.atena.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class DespesaViewModel: AbstractEntityViewModel
    {
        public DespesaViewModel()
        {
            FluxoDeCaixasDespesas = new HashSet<FluxoDeCaixaDespesaViewModel>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool DespesaFixa { get; set; }

        public Guid CompetenciaId { get; set; }

        public virtual CompetenciaViewModel Competencia { get; set; }
        public virtual ICollection<FluxoDeCaixaDespesaViewModel> FluxoDeCaixasDespesas { get; set; }
    }
}
