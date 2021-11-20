using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Account
{
    public class Divida : AbstractEntity
    {
        public EnumTipoDivida EnumTipoDivida { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataQueDeviaTerRecebido { get; set; }
        public DateTime DataCompra { get; set; }
        public Guid? PessoaId { get; set; }
        public Guid? FornecedorId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual Competencia Competencia { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
