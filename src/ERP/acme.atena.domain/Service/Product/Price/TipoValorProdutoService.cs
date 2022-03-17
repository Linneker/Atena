using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Product.Price
{
    public class TipoValorProdutoService : ServiceBase<TipoValorProduto>, ITipoValorProdutoService
    {
        private readonly ITipoValorProdutoRepository _tipoValorProdutoRepository;

        public TipoValorProdutoService(ITipoValorProdutoRepository tipoValorProdutoRepository) : base(tipoValorProdutoRepository)
        {
            _tipoValorProdutoRepository = tipoValorProdutoRepository;
        }

        public Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome)
        {
            return _tipoValorProdutoRepository.GetTipoValorProdutoByNomeAsync(nome);
        }
    }
}
