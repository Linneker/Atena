using imagem.bar.francisco.domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Interface.Repository.Account
{
    public interface IDespesaRepository : IRepositoryBase<Despesa>
    {
        List<Despesa> GetDespesaOrderByMaiorValor();
        Task<List<Despesa>> GetDespesaOrderByMaiorValorAsync();
        List<Despesa> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes);
        Task<List<Despesa>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes);
        List<Despesa> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano);
        Task<List<Despesa>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano);
        List<Despesa> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes);
        Task<List<Despesa>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes);
        List<Despesa> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes);
        Task<List<Despesa>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes);
    }
}
