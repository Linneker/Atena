
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Account
{
    public class DividaService: ServiceBase<Divida>, IDividaService
    {
        private readonly IDividaRepository _dividaService;

        public DividaService(IDividaRepository dividaService):base(dividaService)
        {
            _dividaService = dividaService;
        }
    }
}
