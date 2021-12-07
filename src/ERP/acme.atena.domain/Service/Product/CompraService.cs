using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Product
{
    public class CompraService : ServiceBase<Compra>, ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IProdutoService _produtoService;

        public CompraService(ICompraRepository repositoryBase,
            IEstoqueService estoqueService,
            IProdutoService produtoService) : base(repositoryBase)
        {
            _compraRepository = repositoryBase;
            _estoqueService = estoqueService;
            _produtoService = produtoService;
        }

        public void CadastroAsync(Compra compra, Guid empresaId)
        {
            List<Estoque> estoques = _estoqueService.GetEstoquesByEmpressId(empresaId);

            Parallel.ForEach(estoques, e =>
          {
              Parallel.ForEach(compra.ComprasProdutos, async t =>
              {
                  long totalProduto = _produtoService.GetTotalProdutoByEstoqueId(e.Id);
                  if (e.EstoqueMaximo > totalProduto)
                  {
                      Produto produto = t.Produto is null ? await _produtoService.GetProdutoJoinEstoqueProdutoByIdAsync(t.ProdutoId) : t.Produto;
                      if (produto is not null)
                      {
                          if (produto.EstoqueProdutos.FirstOrDefault().EstoqueId == e.Id)
                          {
                              if (produto.EstoqueProdutos.FirstOrDefault().QuantidadeMaxima > t.QuantidadeComprada && e.EstoqueMaximo > (totalProduto + t.QuantidadeComprada))
                              {
                                  this.AddAsync(compra);
                              }
                          }
                      }
                  }

              });
          });

        }
    }
}
