using acme.atena.api.ViewModel.Person;
using acme.atena.api.ViewModel.Security;
using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Account
{
    public class PagamentoViewModel : AbstractEntityViewModel
    {
        private int _numeroDaParcela = 0;
        public PagamentoViewModel()
        {
            if (!Parcelado)
                _numeroDaParcela = 1;

            if (DataCredito is null || DataCredito == new DateTime(1, 1, 1))
                DataCredito = DataPagamento.AddDays(2);
        }

        public bool Parcelado { get; set; }
        public int NumeroDaParcela { get => _numeroDaParcela; set => _numeroDaParcela = Parcelado ? value : _numeroDaParcela; }
        public DateTime DataPagamento { get; set; }
        public DateTime? DataCredito { get; set; }
        public decimal ValorPago { get; set; }
        public EnumTipoPagamento EnumTipoPagamento { get; set; }
        public Guid? UsuarioId { get; set; }
        public Guid? FornecedorId { get; set; }
        public Guid? ClienteId { get; set; }
        public Guid? EmpresaId { get; set; }
        public Guid? PagamentoVendaId { get; set; }

        public virtual ClienteViewModel Cliente { get; set; }
        public virtual FornecedorViewModel Fornecedor { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual EmpresaViewModel Empresa { get; set; }
        public virtual PagamentoVendaViewModel PagamentoVenda { get; set; }

        public virtual ICollection<PagamentoFormaPagamentoViewModel> PagamentosFormasPagamentos { get; set; } = new HashSet<PagamentoFormaPagamentoViewModel>();


    }
}
