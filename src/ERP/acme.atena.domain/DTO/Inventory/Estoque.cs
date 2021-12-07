using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Inventory
{
    public  class Estoque: AbstractEntity
    {
        public Estoque()
        {
            EntradaProdutoEstoques = new HashSet<EntradaProdutoEstoque>();
            SaidaProdutoEstoques = new HashSet<SaidaProdutoEstoque>();
            EstoqueProdutos = new HashSet<EstoqueProduto>();
        }

        public string Nome { get; set; }
        public bool IsPrincipal { get; set; }
        public int EstoqueMaximo { get; set; }
        public int EstoqueMinimo { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        
        public virtual ICollection<EntradaProdutoEstoque> EntradaProdutoEstoques { get; set; }
        public virtual ICollection<SaidaProdutoEstoque> SaidaProdutoEstoques { get; set; }
        public virtual ICollection<EstoqueProduto> EstoqueProdutos { get; set; }
        
    }
}
