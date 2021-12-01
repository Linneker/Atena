using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Account
{
    public class Pagamento : AbstractEntity
    {
        public Pagamento()
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
        public Guid ClienteId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual Competencia Competencia { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
