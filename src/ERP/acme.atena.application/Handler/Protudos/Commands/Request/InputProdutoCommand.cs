using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Produtos.Commands.Request
{
    public class InputProdutoCommand
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime? Validade { get; set; }
        public Guid TipoProdutoId { get; set; }
    }
}
