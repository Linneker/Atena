using acme.cashflow.api.ViewModel.Account;
using acme.cashflow.application.Interface.Account;
using acme.cashflow.domain.DTO.Account;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividaController : BaseApiController<Divida,DividaViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDividaApplication _dividaApplication;
        private const string NOME_SERVICO = "DIVIDA";

        public DividaController(IMapper mapper, IDividaApplication dividaApplication):base(mapper,dividaApplication,NOME_SERVICO)
        {
            _mapper = mapper;
            _dividaApplication = dividaApplication;
        }
    }
}
