using acme.atena.application.Interface.Account;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Account
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
