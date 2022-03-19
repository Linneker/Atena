using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Product
{
    public class VendaProdutoService : ServiceBase<VendaProduto>, IVendaProdutoService
    {
        private readonly IVendaProdutoRepository _vendaProdutoRepository;
        private readonly ISaidaProdutoEstoqueService _saidaProdutoEstoqueService;
        private readonly IEstoqueService _estoqueService;
        private readonly IProdutoService _produtoService;
        private readonly IEstoqueProdutoService _estoqueProdutoService;

        public VendaProdutoService(IVendaProdutoRepository vendaProdutoRepository,
            ISaidaProdutoEstoqueService saidaProdutoEstoqueService,
            IEstoqueService estoqueService,
            IProdutoService produtoService,
            IEstoqueProdutoService estoqueProdutoService) : base(vendaProdutoRepository)
        {
            _vendaProdutoRepository = vendaProdutoRepository;
            _saidaProdutoEstoqueService = saidaProdutoEstoqueService;
            _estoqueService = estoqueService;
            _produtoService = produtoService;
            _estoqueProdutoService = estoqueProdutoService;
        }
        public async void Enviado(VendaProduto vendaProduto, Guid estoqueId)
        {
            EstoqueProduto estoqueProdutoService = await _estoqueProdutoService.GetEstoqueProdutoByProdutoIdAsync(vendaProduto.ProdutoId);
            if (estoqueProdutoService is null)
            {
                throw (new Exception("Por favor, cadastre o produto nesse estoque, depois realize a operação de envio!"));
            }
            estoqueProdutoService.QuantidadeProduto = estoqueProdutoService.QuantidadeProduto - vendaProduto.QuantidadeVedida;
            VendaProduto vendaProdutoAntiga = await this.GetByIdAsync(vendaProduto.Id);
            vendaProdutoAntiga.Enviado = true;

            SaidaProdutoEstoque entradaProdutoEstoque = new SaidaProdutoEstoque(vendaProduto.ProdutoId, estoqueId, vendaProduto.QuantidadeVedida);
            _saidaProdutoEstoqueService.Add(entradaProdutoEstoque);


        }
    }
}
