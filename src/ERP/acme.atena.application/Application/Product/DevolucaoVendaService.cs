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

namespace acme.atena.application.Service.Product
{
    public class DevolucaoVendaService : ServiceBase<DevolucaoVenda>, IDevolucaoVendaService
    {
        private readonly IDevolucaoVendaRepository _devolucaoVendaRepository;
        private readonly IEstoqueProdutoService _estoqueProdutoService;
        private readonly IVendaProdutoService _vendaProdutoService;
        private readonly IEntradaProdutoEstoqueService _entradaProdutoEstoqueService;

        public DevolucaoVendaService(IDevolucaoVendaRepository devolucaoVendaRepository, IEstoqueProdutoService estoqueProdutoService, IVendaProdutoService vendaProdutoService, IEntradaProdutoEstoqueService entradaProdutoEstoqueService) : base(devolucaoVendaRepository)
        {
            _devolucaoVendaRepository = devolucaoVendaRepository;
            _estoqueProdutoService = estoqueProdutoService;
            _vendaProdutoService = vendaProdutoService;
            _entradaProdutoEstoqueService = entradaProdutoEstoqueService;
        }

        public void Devolver(List<DevolucaoVenda> devolucaoVenda)
        {
            Parallel.ForEach(devolucaoVenda, async t =>
            {
                EstoqueProduto estoqueProduto = await _estoqueProdutoService.GetEstoqueProdutoByProdutoIdAsync(t.ProdutoId);
                VendaProduto vendaProd = new VendaProduto();
                vendaProd = (t.VendaProduto is null)
                ? await _vendaProdutoService.GetByIdAsync(t.VendaProdutoId)
                : t.VendaProduto;
                estoqueProduto.QuantidadeProduto = estoqueProduto.QuantidadeProduto + vendaProd.QuantidadeVedida;

                EntradaProdutoEstoque entradaProdutoEstoque = new EntradaProdutoEstoque(t.ProdutoId, estoqueProduto.EstoqueId);
                _entradaProdutoEstoqueService.AddAsync(entradaProdutoEstoque);

                AddAsync(t);
            });
        }
    }
}
