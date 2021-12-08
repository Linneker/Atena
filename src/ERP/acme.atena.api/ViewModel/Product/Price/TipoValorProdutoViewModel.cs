using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Product.Price
{
    public class TipoValorProdutoViewModel : AbstractEntityViewModel
    {
        public TipoValorProdutoViewModel(string nome)
        {
            Nome = nome;
            ValorProdutos = new HashSet<ValorProdutoViewModel>();
        }

        public string Nome { get; set; }
        public virtual ICollection<ValorProdutoViewModel> ValorProdutos { get; set; }
    }
}
