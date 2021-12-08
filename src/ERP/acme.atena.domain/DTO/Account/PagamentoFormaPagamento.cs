using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Account
{
    public class PagamentoFormaPagamento : AbstractEntity
    {
        public PagamentoFormaPagamento()
        {
        }

        public PagamentoFormaPagamento(Guid pagamentoId, Guid formaPagamentoId)
        {
            PagamentoId = pagamentoId;
            FormaPagamentoId = formaPagamentoId;
        }
        public PagamentoFormaPagamento(Guid formaPagamentoId)
        {
            FormaPagamentoId = formaPagamentoId;
        }
        public Guid PagamentoId { get; set; }
        public Guid FormaPagamentoId { get; set; }

        public virtual Pagamento Pagamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }


    }
}
