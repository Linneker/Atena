using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Account
{
    public class PagamentoVendaViewModel : AbstractEntityCompetenciaViewModel
    {
        private int _diaVencimento=0;
        private int _numeroDaParcela = 0;
        private DateTime _dataCredito;
        public PagamentoVendaViewModel()
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
        public virtual ICollection<PagamentoVendaFormaPagamentoViewModel> PagamentoVendasFormasPagamentos { get; set; } = new HashSet<PagamentoVendaFormaPagamentoViewModel>();
        public virtual ICollection<PagamentoViewModel> Pagamentos { get; set; } = new HashSet<PagamentoViewModel>();

    }
}
