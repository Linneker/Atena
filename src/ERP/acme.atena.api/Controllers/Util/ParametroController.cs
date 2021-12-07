﻿using acme.atena.api.ViewModel.Util;
using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Util;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Util
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametroController : BaseApiController<Parametro, ParametroViewModel>
    {
        private readonly IParametroApplication _parametroApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PARAMETRO";

        public ParametroController(IParametroApplication parametroApplication, IMapper mapper):base(mapper, parametroApplication,NOME_SERVICO)
        {
            _parametroApplication = parametroApplication;
            _mapper = mapper;
        }
    }
}