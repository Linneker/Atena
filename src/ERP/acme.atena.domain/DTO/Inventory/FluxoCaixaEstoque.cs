using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public class FluxoCaixaEstoque: AbstractEntity
    {
        public decimal ValorTotalEstoqueInicial { get; set; }
        public decimal ValorTotalEntrada { get; set; }
        public decimal ValorTotalSaida { get; set; }
        public decimal ValorTotalEstoqueFinal { get; set; }

        public Guid CompetenciaId { get; set; }
        public Competencia Competencia { get; set; }
    }
}
