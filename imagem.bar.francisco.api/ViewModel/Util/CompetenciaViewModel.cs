using imagem.bar.francisco.api.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.ViewModel.Util
{
    public class CompetenciaViewModel : AbstractEntityViewModel
    {
        public CompetenciaViewModel()
        {
            Despesas = new HashSet<DespesaViewModel>();
            FluxosDeCasas = new HashSet<FluxoDeCaixaViewModel>();
            Receitas = new HashSet<ReceitaViewModel>();
        }

        public int Ano { get; set; }
        public int Mes { get; set; }

        public virtual ICollection<DespesaViewModel> Despesas { get; set; }
        public virtual ICollection<FluxoDeCaixaViewModel> FluxosDeCasas { get; set; }
        public virtual ICollection<ReceitaViewModel> Receitas { get; set; }
    }
}
