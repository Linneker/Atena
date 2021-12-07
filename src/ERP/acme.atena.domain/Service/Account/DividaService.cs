using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Account
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
