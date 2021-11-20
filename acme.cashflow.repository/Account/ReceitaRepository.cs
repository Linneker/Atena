using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Repository.Account;
using acme.cashflow.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.repository.Account
{
    public class ReceitaRepository : RepositoryBase<Receita>, IReceitaRepository
    {
        public ReceitaRepository(Context db) : base(db)
        {
        }

        public List<Receita> GetReceitasOrderByMaiorValor()
        {
            var query = _db.Receitas.OrderBy(t => t.Valor).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitasOrderByMaiorValorAsync()
        {
            var query = _db.Receitas.OrderByDescending(t => t.Valor).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Receita>> GetReceitasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }


        public List<Receita> GetReceitasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Receita>> GetReceitasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitasByCompetenciaId(Guid competenciaId)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where dps.CompetenciaId == competenciaId
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public List<Receita> GetReceitasMaisExpressisvasDoMes()
        {
            int anoAtual = DateTime.Now.Year;
            int mesAtual = DateTime.Now.Month;
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes == mesAtual && comp.Ano == anoAtual
                         orderby dps.Valor descending
                         select dps).Take(10).ToList();
            return query;
        }

        public decimal GetReceitaTotalMes()
        {
            int anoAtual = DateTime.Now.Year;
            int mesAtual = DateTime.Now.Month;
            decimal query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes == mesAtual && comp.Ano == anoAtual
                         orderby dps.Valor 
                         select dps).ToList().Sum(t => t.Valor);
            return query;
        }
        public decimal GetReceitaTotalAnual()
        {
            int anoAtual = DateTime.Now.Year;
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes <= 12 && comp.Ano == anoAtual
                         orderby dps.Valor 
                         select dps).Sum(t => t.Valor); ;
            return query;
        }

        public decimal GetReceitaTotalByMes(int mes)
        {
            int anoAtual = DateTime.Now.Year;
            decimal query = (from dps in _db.Receitas
                             join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                             where comp.Mes == mes && comp.Ano == anoAtual
                             orderby dps.Valor
                             select dps).Sum(t => t.Valor);
            return query;
        }

        public decimal GetReceitaTotalByAno(int ano)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes <= 12 && comp.Ano == ano
                         orderby dps.Valor
                         select dps).Sum(t => t.Valor); ;
            return query;
        }
    }

}
