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

        public ReceitaApplication(IReceitaService receitaService):base(receitaService)
        {
            _receitaService = receitaService;
        }

        public List<Receita> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _receitaService.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaService.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _receitaService.GetReceitaByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Receita>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _receitaService.GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Receita> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _receitaService.GetReceitaByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Receita>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _receitaService.GetReceitaByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Receita> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _receitaService.GetReceitaByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaService.GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitaOrderByMaiorValor()
        {
            return _receitaService.GetReceitaOrderByMaiorValor();
        }

        public Task<List<Receita>> GetReceitaOrderByMaiorValorAsync()
        {
            return _receitaService.GetReceitaOrderByMaiorValorAsync();
        }
    }
}
