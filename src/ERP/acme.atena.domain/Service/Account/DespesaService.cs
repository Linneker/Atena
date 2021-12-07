using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Account
{
    public class DespesaService : ServiceBase<Despesa>, IDespesaService
    {
        private readonly IDespesaRepository _despesaRepository;

        public DespesaService(IDespesaRepository despesaRepository) : base(despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public List<Despesa> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _despesaRepository.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaRepository.GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _despesaRepository.GetDespesasByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _despesaRepository.GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Despesa> GetDespesasByCompetenciaId(Guid guid)
        {
            return _despesaRepository.GetDespesasByCompetenciaId(guid);
        }

        public List<Despesa> GetDespesasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _despesaRepository.GetDespesasByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _despesaRepository.GetDespesasByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Despesa> GetDespesasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _despesaRepository.GetDespesasByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaRepository.GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesasMaisExpressisvasDoMes()
        {
            return _despesaRepository.GetDespesasMaisExpressisvasDoMes();
        }

        public List<Despesa> GetDespesasOrderByMaiorValor()
        {
            return _despesaRepository.GetDespesasOrderByMaiorValor();
        }

        public Task<List<Despesa>> GetDespesasOrderByMaiorValorAsync()
        {
            return _despesaRepository.GetDespesasOrderByMaiorValorAsync();
        }

        public decimal GetDespesaTotalAnual()
        {
            return _despesaRepository.GetDespesaTotalAnual();
        }

        public decimal GetDespesaTotalByAno(int ano)
        {
            return _despesaRepository.GetDespesaTotalAnual();
        }

        public decimal GetDespesaTotalByMes(int mes)
        {
            return _despesaRepository.GetDespesaTotalByMes(mes);
        }

        public decimal GetDespesaTotalMes()
        {
            return _despesaRepository.GetDespesaTotalMes();
        }
    }
}
