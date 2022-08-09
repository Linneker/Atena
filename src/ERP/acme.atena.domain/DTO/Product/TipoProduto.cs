using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Product
{
    public class TipoProduto : AbstractEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
