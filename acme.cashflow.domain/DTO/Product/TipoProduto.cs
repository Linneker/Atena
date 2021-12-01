using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.DTO.Product
{
    public class TipoProduto : AbstractEntity
    {
        public string Nome { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
