using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.repository.Account
{
    public class DespesaRepository : RepositoryBase<Despesa>, IDespesaRepository
    {
        public DespesaRepository(Context db) : base(db)
        {
        }

        public List<Despesa> GetDespesaOrderByMaiorValor()
        {
            var query = _db.Despesas.OrderBy(t => t.Valor).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesaOrderByMaiorValorAsync()
        {
            var query = _db.Despesas.OrderByDescending(t => t.Valor).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }


        public List<Despesa> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Despesa>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Despesa>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Despesa> GetDespesaByCompetenciaId(Guid guid)
        {
            var query = (from dps in _db.Despesas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (dps.CompetenciaId == guid)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
    }
}
