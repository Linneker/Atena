using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class PagamentoFormaPagamentoViewModel : AbstractEntityViewModel
    {
        public PagamentoFormaPagamentoViewModel()
        {
        }

        public PagamentoFormaPagamentoViewModel(Guid pagamentoId, Guid formaPagamentoId)
        {
            PagamentoId = pagamentoId;
            FormaPagamentoId = formaPagamentoId;
        }
        public PagamentoFormaPagamentoViewModel(Guid formaPagamentoId)
        {
            FormaPagamentoId = formaPagamentoId;
        }
        public Guid PagamentoId { get; set; }
        public Guid FormaPagamentoId { get; set; }

        public virtual PagamentoViewModel Pagamento { get; set; }
        public virtual FormaPagamentoViewModel FormaPagamento { get; set; }


    }
}
