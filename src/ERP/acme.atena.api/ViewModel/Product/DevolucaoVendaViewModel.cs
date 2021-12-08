using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Product
{
    public class DevolucaoVendaViewModel : AbstractEntityCompetenciaViewModel
    {
        public string Motivo { get; set; }
        public Guid VendaProdutoId { get; set; }
        public Guid ProdutoId { get; set; }
        public bool IsParcial { get; set; } = false;

        public virtual VendaProdutoViewModel VendaProduto { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }

    }
}
