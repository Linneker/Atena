using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Account
{
    public class Receita : AbstractEntity
    {
        public Receita()
        {
            FluxoDeCaixasReceitas = new HashSet<FluxoDeCaixaReceita>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool ReceitaFixa { get; set; }

        public Guid CompetenciaId { get; set; }

        public virtual Competencia Competencia { get; set; }

        public virtual ICollection<FluxoDeCaixaReceita> FluxoDeCaixasReceitas { get; set; }

    }
}
