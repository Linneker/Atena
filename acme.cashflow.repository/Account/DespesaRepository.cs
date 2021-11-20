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
    public class DespesaRepository : RepositoryBase<Despesa>, IDespesaRepository
    {
        public DespesaRepository(Context db) : base(db)
        {
        }

        public List<Despesa> GetDespesasOrderByMaiorValor()
        {
            var query = _db.Despesas.OrderBy(t => t.Valor).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesasOrderByMaiorValorAsync()
        {
            var query = _db.Despesas.OrderByDescending(t => t.Valor).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Despesa>> GetDespesasByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesasByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesasByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }


        public List<Despesa> GetDespesasByCompetenciaMesOrderByMaiorValor(int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesasByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesasByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Despesa>> GetDespesasByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesasByCompetenciaId(Guid guid)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (dps.CompetenciaId == guid)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public List<Despesa> GetDespesasMaisExpressisvasDoMes()
        {
            int anoAtual = DateTime.Now.Year;
            int mesAtual = DateTime.Now.Month;
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes == mesAtual && comp.Ano == anoAtual
                         orderby dps.Valor descending
                         select dps).Take(10).ToList();
            return query;
        }

        public decimal GetDespesaTotalMes()
        {
            int anoAtual = DateTime.Now.Year;
            int mesAtual = DateTime.Now.Month;
            decimal query = (from dps in _db.Despesas
                             join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                             where comp.Mes == mesAtual && comp.Ano == anoAtual
                             orderby dps.Valor
                             select dps).Sum(t => t.Valor);
            return query;
        }
        public decimal GetDespesaTotalAnual()
        {
            int anoAtual = DateTime.Now.Year;

            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes <= 12 && comp.Ano == anoAtual
                         orderby dps.Valor
                         select dps).Sum(t => t.Valor); ;
            return query;
        }

        public decimal GetDespesaTotalByMes(int mes)
        {
            int anoAtual = DateTime.Now.Year;
            
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes == mes && comp.Ano == anoAtual
                         orderby dps.Valor
                         select dps).Sum(t => t.Valor); ;
            return query;
        }

        public decimal GetDespesaTotalByAno(int ano)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes <= 12 && comp.Ano == ano
                         orderby dps.Valor
                         select dps).Sum(t => t.Valor); ;
            return query;
        }
    }
}
