using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Account
{
    public class FluxoDeCaixa : AbstractEntity
    {
        public FluxoDeCaixa()
        {
            FluxoDeCaixasReceitas = new HashSet<FluxoDeCaixaReceita>();
            FluxoDeCaixasDespesas = new HashSet<FluxoDeCaixaDespesa>();
        }

        public decimal SaldoOperacional { get; set; }
        public decimal SaldoFinalCaixa { get; set; }
        public decimal SaldoInicial { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual Competencia Competencia { get; set; }
        public virtual ICollection<FluxoDeCaixaReceita> FluxoDeCaixasReceitas { get; set; }
        public virtual ICollection<FluxoDeCaixaDespesa> FluxoDeCaixasDespesas { get; set; }
    }
}
