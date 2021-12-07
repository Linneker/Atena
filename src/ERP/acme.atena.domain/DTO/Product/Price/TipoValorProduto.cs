using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Product.Price
{
    public class TipoValorProduto : AbstractEntity
    {
        public TipoValorProduto(string nome)
        {
            Nome = nome;
            ValorProdutos = new HashSet<ValorProduto>();
        }

        public string Nome { get; set; }
        public virtual ICollection<ValorProduto> ValorProdutos { get; set; }
    }
}
