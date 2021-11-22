using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Repository.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Account
{
    public class DividaService: ServiceBase<Divida>, IDividaService
    {
        private readonly IDividaRepository _dividaRepository;

        public DividaService(IDividaRepository dividaRepository):base(dividaRepository)
        {
            _dividaRepository = dividaRepository;
        }
    }
}
