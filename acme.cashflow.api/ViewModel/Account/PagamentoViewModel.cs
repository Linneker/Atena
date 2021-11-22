using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using acme.cashflow.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Account
{
    public class PagamentoViewModel : AbstractEntityViewModel
    {
        public PagamentoViewModel()
        {
            if (!Parcelado)
                NumeroDeParcela = 1;
            
            if(DataCredito is null || DataCredito==new DateTime(1,1,1))
                DataCredito = DataPagamento.AddDays(2);
        }

        public EnumFormaPagamento EnumFormaPagamento { get; set; }
        public bool Parcelado { get; set; }
        public int NumeroDeParcela { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime? DataCredito { get; set; }
        public decimal ValorPago { get; set; }
        public EnumTipoPagamento EnumTipoPagamento { get; set; }
        public Guid? PessoaId { get; set; }
        public Guid? FornecedorId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual CompetenciaViewModel Competencia { get; set; }

        public virtual PessoaViewModel Pessoa { get; set; }
        public virtual FornecedorViewModel Fornecedor { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

    }
}
