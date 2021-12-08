using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Product
{
    public class DevolucaoCompraViewModel : AbstractEntityCompetenciaViewModel
    {
        public string Motivo { get; set; }
        public Guid CompraProdutoId { get; set; }
        public Guid ProdutoId { get; set; }
        public bool IsParcial { get; set; } = false;

        public virtual CompraProdutoViewModel CompraProduto { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }
    }
}
