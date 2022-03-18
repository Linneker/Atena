using acme.atena.api.ViewModel.Account;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Service.Account;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividaController : BaseApiController<Divida,DividaViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDividaService _dividaApplication;
        private const string NOME_SERVICO = "DIVIDA";

        public DividaController(IMapper mapper, IDividaService dividaApplication):base(mapper,dividaApplication,NOME_SERVICO)
        {
            _mapper = mapper;
            _dividaApplication = dividaApplication;
        }
    }
}
