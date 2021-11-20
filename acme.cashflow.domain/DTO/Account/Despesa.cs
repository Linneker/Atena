using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Account
{
    public class Despesa : AbstractEntity
    {
        public Despesa()
        {
            FluxoDeCaixasDespesas = new HashSet<FluxoDeCaixaDespesa>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool DespesaFixa { get; set; }

        public Guid CompetenciaId { get; set; }

        public virtual Competencia Competencia { get; set; }
        public virtual ICollection<FluxoDeCaixaDespesa> FluxoDeCaixasDespesas { get; set; }

    }
}
