using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class FormaPagamentoViewModel: AbstractEntityViewModel
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }

        public virtual ICollection<PagamentoFormaPagamentoViewModel> PagamentoFormaPagamentos { get; set; } = new HashSet<PagamentoFormaPagamentoViewModel>();
        public virtual ICollection<PagamentoVendaFormaPagamentoViewModel> PagamentoVendasFormasPagamentos { get; set; } = new HashSet<PagamentoVendaFormaPagamentoViewModel>();

    }
}
