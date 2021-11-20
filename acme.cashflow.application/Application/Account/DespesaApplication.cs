using acme.cashflow.application.Interface.Account;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.application.Application.Account
{
    public class DespesaApplication : ApplicationBase<Despesa>, IDespesaApplication
    {
        private readonly IDespesaService _despesaService;

        public DespesaApplication(IDespesaService despesaService):base(despesaService)
        {
            _despesaService = despesaService;
        }

        public List<Despesa> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _despesaService.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaService.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _despesaService.GetDespesasByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _despesaService.GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Despesa> GetDespesasByCompetenciaId(Guid guid)
        {
            return _despesaService.GetDespesasByCompetenciaId(guid);
        }

        public List<Despesa> GetDespesasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _despesaService.GetDespesasByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _despesaService.GetDespesasByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Despesa> GetDespesasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _despesaService.GetDespesasByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaService.GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesasMaisExpressisvasDoMes()
        {
            return _despesaService.GetDespesasMaisExpressisvasDoMes();
        }

        public List<Despesa> GetDespesasOrderByMaiorValor()
        {
            return _despesaService.GetDespesasOrderByMaiorValor();
        }

        public Task<List<Despesa>> GetDespesasOrderByMaiorValorAsync()
        {
            return _despesaService.GetDespesasOrderByMaiorValorAsync();
        }

        public decimal GetDespesaTotalAnual()
        {
            return _despesaService.GetDespesaTotalAnual();
        }

        public decimal GetDespesaTotalByAno(int ano)
        {
            return _despesaService.GetDespesaTotalByAno(ano);
        }

        public decimal GetDespesaTotalByMes(int mes)
        {
            return _despesaService.GetDespesaTotalByMes(mes);
        }

        public decimal GetDespesaTotalMes()
        {
            return _despesaService.GetDespesaTotalMes();
        }
    }
}
