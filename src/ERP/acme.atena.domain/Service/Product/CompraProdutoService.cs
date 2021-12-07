using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Product
{
    public class CompraProdutoService : ServiceBase<CompraProduto>, ICompraProdutoService
    {
        private readonly ICompraProdutoRepository _compraProdutoRepository;
        private readonly IEntradaProdutoEstoqueService _entradaProdutoEstoqueService;
        private readonly IEstoqueService _estoqueService;
        private readonly IEstoqueProdutoService _estoqueProdutoService;
        private readonly IProdutoService _produtoService;

        public CompraProdutoService(ICompraProdutoRepository repositoryBase,
            IEntradaProdutoEstoqueService entradaProdutoEstoqueService,
            IEstoqueService estoqueService,
            IEstoqueProdutoService estoqueProdutoService,
            IProdutoService produtoService) : base(repositoryBase)
        {
            _compraProdutoRepository = repositoryBase;
            _entradaProdutoEstoqueService = entradaProdutoEstoqueService;
            _estoqueService = estoqueService;
            _estoqueProdutoService = estoqueProdutoService;
            _produtoService = produtoService;
        }

        public async void Recebido(CompraProduto compraProduto, Guid estoqueId)
        {
            EstoqueProduto estoqueProdutoService = await _estoqueProdutoService.GetEstoqueProdutoByProdutoIdAsync(compraProduto.ProdutoId);
            if (estoqueProdutoService is null)
            {
                throw (new Exception("Por favor, cadastre o produto nesse estoque, depois realize a operação de recebimento!"));
            }
            estoqueProdutoService.QuantidadeProduto += compraProduto.QuantidadeComprada;
            CompraProduto compraProdutoAntiga = await this.GetByIdAsync(compraProduto.Id);
            compraProdutoAntiga.IsRecebido = true;

            EntradaProdutoEstoque entradaProdutoEstoque = new EntradaProdutoEstoque(compraProduto.ProdutoId, estoqueId);
            _entradaProdutoEstoqueService.Add(entradaProdutoEstoque);
        }
    }
}
