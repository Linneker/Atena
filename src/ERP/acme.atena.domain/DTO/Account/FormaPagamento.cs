using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Account
{
    public class FormaPagamento: AbstractEntity
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }

        public virtual ICollection<PagamentoFormaPagamento> PagamentoFormaPagamentos { get; set; } = new HashSet<PagamentoFormaPagamento>();
        public virtual ICollection<PagamentoVendaFormaPagamento> PagamentoVendasFormasPagamentos { get; set; } = new HashSet<PagamentoVendaFormaPagamento>();

    }
}
