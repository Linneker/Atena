using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Product
{
    public class VendaService : ServiceBase<Venda>, IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IProdutoService _produtoService;
        private readonly ISaidaProdutoEstoqueService _saidaProdutoEstoqueService;

        public VendaService(IVendaRepository vendaRepository,
            IEstoqueService estoqueService, IProdutoService produtoService,
            ISaidaProdutoEstoqueService saidaProdutoEstoqueService) : base(vendaRepository)
        {
            _vendaRepository = vendaRepository;
            _estoqueService = estoqueService;
            _produtoService = produtoService;
            _saidaProdutoEstoqueService = saidaProdutoEstoqueService;
        }

        public void Conclusao(Venda venda, Guid empresaId)
        {
            List<Estoque> estoques = _estoqueService.GetEstoquesByEmpressId(empresaId);
            Parallel.ForEach(estoques, e =>
            {
                long totalProduto = _produtoService.GetTotalProdutoByEstoqueId(e.Id);
                Parallel.ForEach(venda.VendasProdutos, async t =>
                {
                    Produto produto = t.Produto is null ? await _produtoService.GetProdutoJoinEstoqueProdutoByIdAsync(t.ProdutoId) : t.Produto;
                    if (produto is not null && produto.EstoqueProdutos.Sum(t => t.QuantidadeProduto) >= t.QuantidadeVedida)
                    {
                        if (produto.EstoqueProdutos.Where(f => f.ProdutoId == t.ProdutoId).FirstOrDefault().QuantidadeProduto >= t.QuantidadeVedida)
                        {
                            this.AddAsync(venda);
                        }
                    }
                    else if (produto is not null && produto.EstoqueProdutos.Sum(t => t.QuantidadeProduto) < t.Produto.ComprasProdutos.FirstOrDefault().QuantidadeComprada)
                    {
                        throw (new Exception($"Não há produto suficiente " + (estoques.Count > 1 ? "nos Estoques" : "no Estoque") + "!"));
                    }
                });


            });
        }
    }
}
