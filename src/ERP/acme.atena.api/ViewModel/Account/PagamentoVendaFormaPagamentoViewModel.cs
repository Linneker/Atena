using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class PagamentoVendaFormaPagamentoViewModel : AbstractEntityViewModel
    {
        public Guid PagamentoVendaId { get; set; }
        public Guid FormaPagamentoId { get; set; }

        public virtual PagamentoVendaViewModel PagamentoVenda { get; set; }
        public virtual FormaPagamentoViewModel FormaPagamento { get; set; }
    }
}
