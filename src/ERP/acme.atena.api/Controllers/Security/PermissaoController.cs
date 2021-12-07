using acme.atena.api.ViewModel.Security;
using acme.atena.application.Interface.Security;
using acme.atena.domain.DTO.Seguranca;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissaoController : BaseApiController<Permissao,PermissaoViewModel>
    {
        private readonly IPermissaoApplication _permissaoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PERMISSÃO";

        public PermissaoController(IPermissaoApplication permissaoApplication, IMapper mapper):base(mapper, permissaoApplication, NOME_SERVICO)
        {
            _permissaoApplication = permissaoApplication;
            _mapper = mapper;
        }
    }
}
