using AutoMapper;
using imagem.bar.francisco.api.ViewModel.Account;
using imagem.bar.francisco.application.Interface.Util;
using imagem.bar.francisco.domain.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : BaseApiController<Receita,ReceitaViewModel>
    {
        private IMapper _mapper;
        private IReceitaApplication _receitaApplication;
        private const string NOME_SERVICO = "RECEITA";
        public ReceitaController(IMapper mapper, IReceitaApplication receitaApplication):base(mapper, receitaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _receitaApplication = receitaApplication;
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor/{ano}/{mes}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoOrderByMaiorValor/{ano}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitaByCompetenciaAnoOrderByMaiorValor(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoOrderByMaiorValorAsync/{ano}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaMesOrderByMaiorValor/{mes}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitaByCompetenciaMesOrderByMaiorValor(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaMesOrderByMaiorValorAsync/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitaByCompetenciaMesOrderByMaiorValorAsync(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByPeriodoCompetenciaOrderByMaiorValor/{ano}/{mes}")]
        public List<ReceitaViewModel> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitaByPeriodoCompetenciaOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaOrderByMaiorValor")]
        public List<ReceitaViewModel> GetReceitaOrderByMaiorValor()
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitaOrderByMaiorValor());
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaOrderByMaiorValorAsync")]
        public Task<List<ReceitaViewModel>> GetReceitaOrderByMaiorValorAsync()
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitaOrderByMaiorValorAsync());
        }
    }
}
