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
    public class ReceitaController : BaseApiController<Receita,ReceitaViewModel>
    {
        private IMapper _mapper;
        private IReceitaService _receitaApplication;
        private const string NOME_SERVICO = "RECEITA";
        public ReceitaController(IMapper mapper, IReceitaService receitaApplication):base(mapper, receitaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _receitaApplication = receitaApplication;
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor/{ano}/{mes}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoOrderByMaiorValor/{ano}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasByCompetenciaAnoOrderByMaiorValor(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaAnoOrderByMaiorValorAsync/{ano}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(ano));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaMesOrderByMaiorValor/{mes}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasByCompetenciaMesOrderByMaiorValor(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaMesOrderByMaiorValorAsync/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitasByCompetenciaMesOrderByMaiorValorAsync(mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByPeriodoCompetenciaOrderByMaiorValor/{ano}/{mes}")]
        public List<ReceitaViewModel> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasByPeriodoCompetenciaOrderByMaiorValor(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync/{ano}/{mes}")]
        public Task<List<ReceitaViewModel>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes));
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaOrderByMaiorValor")]
        public List<ReceitaViewModel> GetReceitaOrderByMaiorValor()
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasOrderByMaiorValor());
        }
        [Authorize("Bearer")]
        [HttpGet("GetReceitaOrderByMaiorValorAsync")]
        public Task<List<ReceitaViewModel>> GetReceitaOrderByMaiorValorAsync()
        {
            return _mapper.Map<Task<List<ReceitaViewModel>>>(_receitaApplication.GetReceitasOrderByMaiorValorAsync());
        }

        [Authorize("Bearer")]
        [HttpGet("GetReceitaByCompetenciaId/{competenciaId}")]
        public List<ReceitaViewModel> GetReceitaByCompetenciaId(Guid competenciaId)
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasByCompetenciaId(competenciaId));
        }


        [Authorize("Bearer")]
        [HttpGet("GetReceitasMaisExpressisvasDoMes")]
        public List<ReceitaViewModel> GetReceitasMaisExpressisvasDoMes()
        {
            return _mapper.Map<List<ReceitaViewModel>>(_receitaApplication.GetReceitasMaisExpressisvasDoMes());
        }

        [Authorize("Bearer")]
        [HttpGet("GetReceitaTotalMes")]
        public decimal GetDespesaTotalMes()
        {
            return _receitaApplication.GetReceitaTotalMes();
        }

        [Authorize("Bearer")]
        [HttpGet("GetReceitaTotalAnual")]
        public decimal GetReceitaTotalAnual()
        {
            return _receitaApplication.GetReceitaTotalAnual();
        }

        [Authorize("Bearer")]
        [HttpGet("GetReceitaTotalByMes/{mes}")]
        public decimal GetReceitaTotalByMes(int mes)
        {
            return _receitaApplication.GetReceitaTotalByMes(mes);
        }

        [Authorize("Bearer")]
        [HttpGet("GetReceitaTotalByAno/{ano}")]
        public decimal GetReceitaTotalByAno(int ano)
        {
            return _receitaApplication.GetReceitaTotalByAno(ano);
        }

    }
}
