using AutoMapper;
using imagem.bar.francisco.api.ViewModel.Util;
using imagem.bar.francisco.application.Interface.Util;
using imagem.bar.francisco.domain.DTO.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.Controllers.Util
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetenciaController : BaseApiController<Competencia,CompetenciaViewModel>
    {
        private IMapper _mapper;
        private ICompetenciaApplication _competenciaApplication;
        private const string NOME_SERVICO = "COMPETENCIA";
        public CompetenciaController(IMapper mapper, ICompetenciaApplication competenciaApplication) : base(mapper, competenciaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _competenciaApplication = competenciaApplication;
        }
    }
}
