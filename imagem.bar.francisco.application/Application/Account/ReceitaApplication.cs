using imagem.bar.francisco.application.Interface.Util;
using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.application.Application.Account
{
    public class ReceitaApplication : ApplicationBase<Receita>, IReceitaApplication
    {
        private readonly IReceitaService _receitaService;
        private readonly ICompetenciaApplication _competenciaApplication;
        public ReceitaApplication(IReceitaService receitaService, ICompetenciaApplication competenciaApplication) :base(receitaService)
        {
            _receitaService = receitaService;
            _competenciaApplication = competenciaApplication;
        }

        public List<Receita> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _receitaService.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaService.GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _receitaService.GetReceitasByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Receita>> GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _receitaService.GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Receita> GetReceitasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _receitaService.GetReceitasByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Receita>> GetReceitasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _receitaService.GetReceitasByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Receita> GetReceitasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _receitaService.GetReceitasByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaService.GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitasByCompetenciaId(Guid competenciaId)
        {
            return _receitaService.GetReceitasByCompetenciaId(competenciaId);
        }

        public List<Receita> GetReceitasOrderByMaiorValor()
        {
            return _receitaService.GetReceitasOrderByMaiorValor();
        }

        public Task<List<Receita>> GetReceitasOrderByMaiorValorAsync()
        {
            return _receitaService.GetReceitasOrderByMaiorValorAsync();
        }

        public List<Receita> GetReceitasMaisExpressisvasDoMes()
        {
            return _receitaService.GetReceitasMaisExpressisvasDoMes();
        }

        public decimal GetReceitaTotalMes()
        {
            return _receitaService.GetReceitaTotalMes();
        }

        public decimal GetReceitaTotalAnual()
        {
            return _receitaService.GetReceitaTotalAnual();
        }

        public decimal GetReceitaTotalByMes(int mes)
        {
            return _receitaService.GetReceitaTotalByMes(mes);
        }

        public decimal GetReceitaTotalByAno(int ano)
        {
            return _receitaService.GetReceitaTotalByAno(ano);
        }
    }
}
