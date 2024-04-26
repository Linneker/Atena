using acme.atena.application.Handler.Comum.Commands.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Produtos.Commands.Response
{
    public class OutputProdutoCommand: OutputAbstractEntityCommand
    {
        public string Nome { get; set; }
        public long Codigo { get; set; }
        public string Descricao { get; set; }
        public DateTime? Validade { get; set; }
        public string TipoProduto { get; set; }
    }
}
