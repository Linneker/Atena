using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Product
{
    public class TipoProdutoService: ServiceBase<TipoProduto>, ITipoProdutoService
    {
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public TipoProdutoService(ITipoProdutoRepository repositoryBase) : base(repositoryBase)
        {
        }
    }
}
