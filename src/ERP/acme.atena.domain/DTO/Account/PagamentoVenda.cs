using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Account
{
    public class PagamentoVenda : AbstractEntityCompetencia
    {
        private int _diaVencimento=0;
        private int _numeroDaParcela = 0;
        private DateTime _dataCredito;
        public PagamentoVenda()
        {
        }

        public EnumStatusPagamento StatusPagamento { get; set; }
        public bool Parcelado { get; set; }
        public int NumeroDeParcela { get => _numeroDaParcela; set => _numeroDaParcela = Parcelado ? value : 1; }
        public int DiaVencimentoParcela { get=> _diaVencimento; set=> _diaVencimento = Parcelado ? value : DateTime.Now.AddDays(2).Day; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataCredito { get=> _dataCredito; set => _dataCredito = DataPagamento.AddDays(2); }

        public Guid VendaId { get; set; }

        public Venda Venda { get; set; }
        public virtual ICollection<PagamentoVendaFormaPagamento> PagamentoVendasFormasPagamentos { get; set; } = new HashSet<PagamentoVendaFormaPagamento>();
        public virtual ICollection<Pagamento> Pagamentos { get; set; } = new HashSet<Pagamento>();

    }
}
