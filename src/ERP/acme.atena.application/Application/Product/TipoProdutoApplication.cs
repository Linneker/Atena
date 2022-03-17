using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Product
{
    public class TipoProdutoApplication: ApplicationBase<TipoProduto>, ITipoProdutoApplication
    {
        private readonly ITipoProdutoService _tipoProdutoService;

        public TipoProdutoApplication(ITipoProdutoService repositoryBase) : base(repositoryBase)
        {
            _tipoProdutoService = repositoryBase;
        }
    }
}
