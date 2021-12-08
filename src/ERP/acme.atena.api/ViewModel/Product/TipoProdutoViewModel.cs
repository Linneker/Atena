using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Product
{
    public class TipoProdutoViewModel : AbstractEntityViewModel
    {
        public string Nome { get; set; }
        public virtual ICollection<ProdutoViewModel> Produtos { get; set; }
    }
}
