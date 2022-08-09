using System;
using System.Collections.Generic;

namespace acme.atena.domain.DTO.Product.Sequence
{
    public class CodigoProduto 
    {
        public CodigoProduto()
        {
            DataCriacao = DateTime.Now;
        }
        public long Id { get; set; }
        public DateTime? DataCriacao { get; set; }

        public ICollection<Produto> Produtos { get; set; } = new HashSet<Produto>();
    }
}
