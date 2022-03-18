using AutoMapper;
using acme.atena.api.ViewModel.Account;
using acme.atena.domain.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acme.atena.domain.Interface.Service.Account;

namespace acme.atena.api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesaController : BaseApiController<Despesa,DespesaViewModel>
    {
        private IMapper _mapper;
        private IDespesaService _despesaApplication;
        private const string NOME_SERVICO = "DESPESA";
        public DespesaController(IMapper mapper, IDespesaService despesaApplication) : base(mapper, despesaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _despesaApplication = despesaApplication;
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor/{ano}/{mes}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoOrderByMaiorValor/{ano}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasByCompetenciaAnoOrderByMaiorValor(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaAnoOrderByMaiorValorAsync/{ano}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaMesOrderByMaiorValor/{mes}")]
        public List<DespesaViewModel> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasByCompetenciaMesOrderByMaiorValor(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaMesOrderByMaiorValorAsync/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesasByCompetenciaMesOrderByMaiorValorAsync(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByPeriodoCompetenciaOrderByMaiorValor/{ano}/{mes}")]
        public List<DespesaViewModel> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasByPeriodoCompetenciaOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<DespesaViewModel>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaOrderByMaiorValor")]
        public List<DespesaViewModel> GetDespesaOrderByMaiorValor()
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasOrderByMaiorValor());
        }
        [Authorize("Bearer")]
        [HttpGet("GetDespesaOrderByMaiorValorAsync")]
        public Task<List<DespesaViewModel>> GetDespesaOrderByMaiorValorAsync()
        {
            return _mapper.Map<Task<List<DespesaViewModel>>>(_despesaApplication.GetDespesasOrderByMaiorValorAsync());
        }

        [Authorize("Bearer")]
        [HttpGet("GetDespesaByCompetenciaId/{competenciaId}")]
        public List<DespesaViewModel> GetDespesaByCompeetenciaId(Guid competenciaId)
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasByCompetenciaId(competenciaId));
        }

        [Authorize("Bearer")]
        [HttpGet("GetDespesaMaisExpressivasDoMes")]
        public List<DespesaViewModel> GetDespesaMaisExpressivasDoMes()
        {
            return _mapper.Map<List<DespesaViewModel>>(_despesaApplication.GetDespesasMaisExpressisvasDoMes());
        }
        
        [Authorize("Bearer")]
        [HttpGet("GetDespesaTotalMes")]
        public decimal GetDespesaTotalMes()
        {
            return _despesaApplication.GetDespesaTotalMes();
        }

        [Authorize("Bearer")]
        [HttpGet("GetDespesaTotalAnual")]
        public decimal GetDespesaTotalAnual()
        {
            return _despesaApplication.GetDespesaTotalAnual();
        }

        [Authorize("Bearer")]
        [HttpGet("GetDespesaTotalByMes/{mes}")]
        public decimal GetDespesaTotalByMes(int mes)
        {
            return _despesaApplication.GetDespesaTotalByMes(mes);
        }

        [Authorize("Bearer")]
        [HttpGet("GetDespesaTotalByAno/{ano}")]
        public decimal GetDespesaTotalByAno(int ano)
        {
            return _despesaApplication.GetDespesaTotalByAno(ano);
        }

    }
}
