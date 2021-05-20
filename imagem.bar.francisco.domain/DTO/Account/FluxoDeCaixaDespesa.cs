using imagem.bar.francisco.domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.DTO.Account
{
    public class FluxoDeCaixaDespesa : AbstractEntity
    {
        public Guid FluxoDeCaixaId { get; set; }
        public Guid DespesaId { get; set; }

        public virtual FluxoDeCaixa FluxoDeCaixa { get; set; }
        public virtual Despesa Despesa { get; set; }
    }
}
