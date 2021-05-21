using imagem.bar.francisco.domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Interface.Repository.Account
{
    public interface IReceitaRepository : IRepositoryBase<Receita>
    {
        List<Receita> GetReceitaOrderByMaiorValor();
        Task<List<Receita>> GetReceitaOrderByMaiorValorAsync();
        List<Receita> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes);
        Task<List<Receita>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes);
        List<Receita> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano);
        Task<List<Receita>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano);
        List<Receita> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes);
        Task<List<Receita>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes);
        List<Receita> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes);
        Task<List<Receita>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes);
        List<Receita> GetReceitaByCompetenciaId(Guid competenciaId);

    }
}
