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
    public class ReceitaRepository : RepositoryBase<Receita>, IReceitaRepository
    {
        public ReceitaRepository(Context db) : base(db)
        {
        }

        public List<Receita> GetReceitaOrderByMaiorValor()
        {
            var query = _db.Receitas.OrderBy(t => t.Valor).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitaOrderByMaiorValorAsync()
        {
            var query = _db.Receitas.OrderByDescending(t => t.Valor).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Receita>> GetReceitaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }


        public List<Receita> GetReceitaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }
        public Task<List<Receita>> GetReceitaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where comp.Mes.Equals(mes)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }

        public List<Receita> GetReceitaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToList();
            return query;
        }

        public Task<List<Receita>> GetReceitaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            var query = (from dps in _db.Receitas
                         join comp in _db.Competencias on dps.CompetenciaId equals comp.Id
                         where (comp.Mes > 0 && comp.Mes <= mes) && comp.Ano.Equals(ano)
                         orderby dps.Valor descending
                         select dps).ToListAsync();
            return query;
        }
    }
}
