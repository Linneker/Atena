using imagem.bar.francisco.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.ViewModel.Account
{
    public class FluxoDeCaixaViewModel: AbstractEntityViewModel
    {
        public FluxoDeCaixaViewModel()
        {
            FluxoDeCaixasReceitas = new HashSet<FluxoDeCaixaReceitaViewModel>();
            FluxoDeCaixasDespesas = new HashSet<FluxoDeCaixaDespesaViewModel>();
        }

        public decimal SaldoOperacional { get; set; }
        public decimal SaldoFinalCaixa { get; set; }
        public decimal SaldoInicial { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual CompetenciaViewModel Competencia { get; set; }
        public virtual ICollection<FluxoDeCaixaReceitaViewModel> FluxoDeCaixasReceitas { get; set; }
        public virtual ICollection<FluxoDeCaixaDespesaViewModel> FluxoDeCaixasDespesas { get; set; }
    }
}
