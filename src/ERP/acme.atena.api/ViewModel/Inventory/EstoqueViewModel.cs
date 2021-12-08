using acme.atena.api.ViewModel.Person;
using acme.atena.api.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.api.ViewModel.Inventory
{
    public  class EstoqueViewModel: AbstractEntityViewModel
    {
        public EstoqueViewModel()
        {
            EntradaProdutoEstoques = new HashSet<EntradaProdutoEstoqueViewModel>();
            SaidaProdutoEstoques = new HashSet<SaidaProdutoEstoqueViewModel>();
            EstoqueProdutos = new HashSet<EstoqueProdutoViewModel>();
        }

        public string Nome { get; set; }
        public bool IsPrincipal { get; set; }
        public int EstoqueMaximo { get; set; }
        public int EstoqueMinimo { get; set; }
        public Guid EmpresaId { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        
        public virtual ICollection<EntradaProdutoEstoqueViewModel> EntradaProdutoEstoques { get; set; }
        public virtual ICollection<SaidaProdutoEstoqueViewModel> SaidaProdutoEstoques { get; set; }
        public virtual ICollection<EstoqueProdutoViewModel> EstoqueProdutos { get; set; }
        
    }
}
