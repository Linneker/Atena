using acme.atena.api.ViewModel.Person;
using acme.atena.api.ViewModel.Security;
using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.ViewModel.Account
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
        public virtual ClienteViewModel Pessoa { get; set; }
        public virtual FornecedorViewModel Fornecedor { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

    }
}
