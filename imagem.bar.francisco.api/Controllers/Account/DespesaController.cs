using AutoMapper;
using imagem.bar.francisco.api.ViewModel.Account;
using imagem.bar.francisco.application.Interface.Account;
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
    public class DespesaController : BaseApiController<Despesa,DespesaViewModel>
    {
        private IMapper _mapper;
        private IDespesaApplication _despesaApplication;
        private const string NOME_SERVICO = "DESPESA";
        public DespesaController(IMapper mapper, IDespesaApplication despesaApplication) : base(mapper, despesaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _despesaApplication = despesaApplication;
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor/{ano}/{mes}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoOrderByMaiorValor/{ano}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesaByCompetenciaAnoOrderByMaiorValor(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoOrderByMaiorValorAsync/{ano}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaMesOrderByMaiorValor/{mes}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesaByCompetenciaMesOrderByMaiorValor(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaMesOrderByMaiorValorAsync/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesaByCompetenciaMesOrderByMaiorValorAsync(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByPeriodoCompetenciaOrderByMaiorValor/{ano}/{mes}")]
        public List<DespesaViewModel> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesaByPeriodoCompetenciaOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaOrderByMaiorValor")]
        public List<DespesaViewModel> GetDespesaOrderByMaiorValor()
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesaOrderByMaiorValor());
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaOrderByMaiorValorAsync")]
        public Task<List<DespesaViewModel>> GetDespesaOrderByMaiorValorAsync()
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesaOrderByMaiorValorAsync());
        }
    }
}
