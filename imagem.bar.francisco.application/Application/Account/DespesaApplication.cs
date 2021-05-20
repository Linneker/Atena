using imagem.bar.francisco.application.Interface.Account;
using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.application.Application.Account
{
    public class DespesaApplication : ApplicationBase<Despesa>, IDespesaApplication
    {
        private readonly IDespesaService _despesaService;

        public DespesaApplication(IDespesaService despesaService):base(despesaService)
        {
            _despesaService = despesaService;
        }

        public List<Despesa> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _despesaService.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaService.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _despesaService.GetDespesaByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _despesaService.GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Despesa> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _despesaService.GetDespesaByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _despesaService.GetDespesaByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Despesa> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _despesaService.GetDespesaByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaService.GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesaOrderByMaiorValor()
        {
            return _despesaService.GetDespesaOrderByMaiorValor();
        }

        public Task<List<Despesa>> GetDespesaOrderByMaiorValorAsync()
        {
            return _despesaService.GetDespesaOrderByMaiorValorAsync();
        }
    }
}
