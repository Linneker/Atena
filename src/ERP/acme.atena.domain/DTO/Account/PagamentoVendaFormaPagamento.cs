using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Account
{
    public class PagamentoVendaFormaPagamento : AbstractEntity
    {
        public Guid PagamentoVendaId { get; set; }
        public Guid FormaPagamentoId { get; set; }

        public virtual PagamentoVenda PagamentoVenda { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
    }
}
