using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Account
{
    public class Divida : AbstractEntity
    {
        public EnumTipoDivida EnumTipoDivida { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataQueDeviaTerRecebido { get; set; }
        public DateTime DataCompra { get; set; }
        public Guid? UsuarioId { get; set; }
        public Guid? FornecedorId { get; set; }
        public Guid? ClienteId { get; set; }
        public Guid CompetenciaId { get; set; }
        public Guid? EmpresaId { get; set; }

        public virtual Competencia Competencia { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
