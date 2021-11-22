using acme.cashflow.application.Interface.Account;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Account
{
    public class DividaApplication: ApplicationBase<Divida>, IDividaApplication
    {
        private readonly IDividaService _dividaService;

        public DividaApplication(IDividaService dividaService):base(dividaService)
        {
            _dividaService = dividaService;
        }
    }
}
