using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using acme.cashflow.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Account
{
    public class DividaViewModel : AbstractEntityViewModel
    {
        public EnumTipoDivida EnumTipoDivida { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataQueDeviaTerRecebido { get; set; }
        public DateTime DataCompra { get; set; }
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
