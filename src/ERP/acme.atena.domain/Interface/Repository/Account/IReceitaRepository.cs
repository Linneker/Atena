using acme.atena.domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository.Account
{
    public interface IReceitaRepository : IRepositoryBase<Receita>
    {
        List<Receita> GetReceitasOrderByMaiorValor();
        Task<List<Receita>> GetReceitasOrderByMaiorValorAsync();
        List<Receita> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes);
        Task<List<Receita>> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes);
        List<Receita> GetReceitasByCompetenciaAnoOrderByMaiorValor(int ano);
        Task<List<Receita>> GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(int ano);
        List<Receita> GetReceitasByCompetenciaMesOrderByMaiorValor(int mes);
        Task<List<Receita>> GetReceitasByCompetenciaMesOrderByMaiorValorAsync(int mes);
        List<Receita> GetReceitasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes);
        Task<List<Receita>> GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes);
        List<Receita> GetReceitasByCompetenciaId(Guid competenciaId);
        List<Receita> GetReceitasMaisExpressisvasDoMes();
        decimal GetReceitaTotalMes();
        decimal GetReceitaTotalAnual();
        decimal GetReceitaTotalByMes(int mes);
        decimal GetReceitaTotalByAno(int ano);

    }
}
