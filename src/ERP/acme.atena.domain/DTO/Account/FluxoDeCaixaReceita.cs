using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Account
{
    public class FluxoDeCaixaReceita : AbstractEntity
    {
        public Guid  FluxoDeCaixaId { get; set; }
        public Guid ReceitaId { get; set; }

        public virtual FluxoDeCaixa FluxoDeCaixa { get; set; }
        
        public virtual Receita Receita { get; set; }
    }
}
