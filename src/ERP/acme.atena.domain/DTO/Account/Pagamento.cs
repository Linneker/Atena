using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Account
{
    public class Pagamento : AbstractEntityCompetencia
    {
        private int _numeroDaParcela = 0;
        public Pagamento()
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

        public virtual Cliente Cliente { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual PagamentoVenda PagamentoVenda { get; set; }

        public virtual ICollection<PagamentoFormaPagamento> Pagamentos { get; set; } = new HashSet<PagamentoFormaPagamento>();

    }
}
