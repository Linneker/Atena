using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Product.Price
{
    public class TipoValorProdutoService: ServiceBase<TipoValorProduto>, ITipoValorProdutoService
    {
        private readonly ITipoValorProdutoRepository _tipoValorProdutoApplication;

        public TipoValorProdutoService(ITipoValorProdutoRepository tipoValorProdutoApplication) : base(tipoValorProdutoApplication)
        {
            _tipoValorProdutoApplication = tipoValorProdutoApplication; 
        }

        public async Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome)
        {
            return await _tipoValorProdutoApplication.GetTipoValorProdutoByNomeAsync(nome);
        }
    }
}
