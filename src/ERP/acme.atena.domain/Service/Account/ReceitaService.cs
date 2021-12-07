using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Account
{
    public class ReceitaService : ServiceBase<Receita>, IReceitaService
    {
        private readonly IReceitaRepository _receitaRepository;

        public ReceitaService(IReceitaRepository receitaRepository) : base(receitaRepository)
        {
            _receitaRepository = receitaRepository;
        }

        public List<Receita> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _receitaRepository.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
                }

        public Task<List<Receita>> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaRepository.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _receitaRepository.GetReceitasByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Receita>> GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _receitaRepository.GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Receita> GetReceitasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _receitaRepository.GetReceitasByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Receita>> GetReceitasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _receitaRepository.GetReceitasByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Receita> GetReceitasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _receitaRepository.GetReceitasByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaRepository.GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitasByCompetenciaId(Guid competenciaId)
        {
            return _receitaRepository.GetReceitasByCompetenciaId(competenciaId);
        }

        public List<Receita> GetReceitasOrderByMaiorValor()
        {
            return _receitaRepository.GetReceitasOrderByMaiorValor();
        }

        public Task<List<Receita>> GetReceitasOrderByMaiorValorAsync()
        {
            return _receitaRepository.GetReceitasOrderByMaiorValorAsync();
        }

        public List<Receita> GetReceitasMaisExpressisvasDoMes()
        {
            return _receitaRepository.GetReceitasMaisExpressisvasDoMes();
        }

        public decimal GetReceitaTotalMes()
        {
            return _receitaRepository.GetReceitaTotalMes();
        }
        public decimal GetReceitaTotalAnual()
        {
            return _receitaRepository.GetReceitaTotalAnual();
        }

        public decimal GetReceitaTotalByMes(int mes)
        {
            return _receitaRepository.GetReceitaTotalByMes(mes);
        }

        public decimal GetReceitaTotalByAno(int ano)
        {
            return _receitaRepository.GetReceitaTotalByAno(ano);
        }
    }
}
