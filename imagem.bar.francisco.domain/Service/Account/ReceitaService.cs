using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Service.Account
{
    public class ReceitaService : ServiceBase<Receita>, IReceitaService
    {
        private readonly IReceitaRepository _receitaRepository;

        public ReceitaService(IReceitaRepository receitaRepository) : base(receitaRepository)
        {
            _receitaRepository = receitaRepository;
        }

        public List<Receita> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _receitaRepository.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
                }

        public Task<List<Receita>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaRepository.GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _receitaRepository.GetReceitaByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Receita>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _receitaRepository.GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Receita> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _receitaRepository.GetReceitaByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Receita>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _receitaRepository.GetReceitaByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Receita> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _receitaRepository.GetReceitaByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Receita>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _receitaRepository.GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Receita> GetReceitaOrderByMaiorValor()
        {
            return _receitaRepository.GetReceitaOrderByMaiorValor();
        }

        public Task<List<Receita>> GetReceitaOrderByMaiorValorAsync()
        {
            return _receitaRepository.GetReceitaOrderByMaiorValorAsync();
        }
    }
}
