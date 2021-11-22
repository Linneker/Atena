using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Product
{
    public class CompraService : ServiceBase<Compra>, ICompraService
    {
        private readonly ICompraRepository _compraRepository;

        public CompraService(ICompraRepository repositoryBase) : base(repositoryBase)
        {
            _compraRepository = repositoryBase;
        }
    }
}
