using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
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
    public class DevolucaoCompraService : ServiceBase<DevolucaoCompra>, IDevolucaoCompraService
    {
        private readonly IDevolucaoCompraRepository _devolucaoCompraRepository;
        private readonly IEstoqueProdutoService _estoqueProdutoService;
        private readonly ISaidaProdutoEstoqueService _saidaProdutoEstoqueService;
        private readonly ICompraProdutoService _compraProdutoService;
        public DevolucaoCompraService(IDevolucaoCompraRepository devolucaoCompraRepository,
            IEstoqueProdutoService estoqueProdutoRepository,
            ISaidaProdutoEstoqueService saidaProdutoEstoqueRepository,
            ICompraProdutoService compraProdutoService) : base(devolucaoCompraRepository)
        {
            _devolucaoCompraRepository = devolucaoCompraRepository;
            _estoqueProdutoService = estoqueProdutoRepository;
            _saidaProdutoEstoqueService = saidaProdutoEstoqueRepository;
            _compraProdutoService = compraProdutoService;
        }

        public void Devolver(List<DevolucaoCompra> devolucaoCompra)
        {
            Parallel.ForEach(devolucaoCompra, async t =>
            {
                EstoqueProduto estoqueProduto = await _estoqueProdutoService.GetEstoqueProdutoByProdutoIdAsync(t.ProdutoId);
                CompraProduto compProd = new CompraProduto();
                compProd = (t.CompraProduto is null)
                ? await _compraProdutoService.GetByIdAsync(t.CompraProdutoId)
                : t.CompraProduto;
                estoqueProduto.QuantidadeProduto = estoqueProduto.QuantidadeProduto - compProd.QuantidadeComprada;

                SaidaProdutoEstoque saidaProdutoEstoque = new SaidaProdutoEstoque(t.ProdutoId, estoqueProduto.EstoqueId, compProd.QuantidadeComprada);
                _saidaProdutoEstoqueService.AddAsync(saidaProdutoEstoque);

                AddAsync(t);
            });
        }
    }
}
