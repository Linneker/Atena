using imagem.bar.francisco.domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Interface.Repository.Account
{
    public interface IDespesaRepository : IRepositoryBase<Despesa>
    {
        List<Despesa> GetDespesasOrderByMaiorValor();
        Task<List<Despesa>> GetDespesasOrderByMaiorValorAsync();
        List<Despesa> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes);
        Task<List<Despesa>> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes);
        List<Despesa> GetDespesasByCompetenciaAnoOrderByMaiorValor(int ano);
        Task<List<Despesa>> GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(int ano);
        List<Despesa> GetDespesasByCompetenciaMesOrderByMaiorValor(int mes);
        Task<List<Despesa>> GetDespesasByCompetenciaMesOrderByMaiorValorAsync(int mes);
        List<Despesa> GetDespesasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes);
        Task<List<Despesa>> GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes);
        List<Despesa> GetDespesasByCompetenciaId(Guid guid);
        List<Despesa> GetDespesasMaisExpressisvasDoMes();
        decimal GetDespesaTotalMes();
        decimal GetDespesaTotalAnual();
        decimal GetDespesaTotalByMes(int mes);
        decimal GetDespesaTotalByAno(int ano);

    }
}
