using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Product
{
    public class DevolucaoVenda : AbstractEntityCompetencia
    {
        public string Motivo { get; set; }
        public Guid VendaProdutoId { get; set; }
        public Guid ProdutoId { get; set; }
        public bool IsParcial { get; set; } = false;

        public virtual VendaProduto VendaProduto { get; set; }
        public virtual Produto Produto { get; set; }

    }
}
