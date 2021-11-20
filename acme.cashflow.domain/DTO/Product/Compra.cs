using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Product
{
    public class Compra : AbstractEntity
    {
        public DateTime DataCompra { get; set; }
        public long ValorTotal { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Competencia Competencia { get; set; }


    }
}
