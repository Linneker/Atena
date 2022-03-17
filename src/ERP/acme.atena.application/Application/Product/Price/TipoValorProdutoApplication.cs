using acme.atena.application.Interface.Product.Price;
using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Product.Price
{
    public class TipoValorProdutoApplication: ServiceBase<TipoValorProduto>, ITipoValorProdutoApplication
    {
        private readonly ITipoValorProdutoService _tipoValorProdutoApplication;

        public TipoValorProdutoApplication(ITipoValorProdutoService tipoValorProdutoApplication) : base(tipoValorProdutoApplication)
        {
            _tipoValorProdutoApplication = tipoValorProdutoApplication; 
        }

        public async Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome)
        {
            return await _tipoValorProdutoApplication.GetTipoValorProdutoByNomeAsync(nome);
        }
    }
}
