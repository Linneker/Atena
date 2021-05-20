using imagem.bar.francisco.domain.DTO.Util;
using imagem.bar.francisco.domain.Interface.Repository.Util;
using imagem.bar.francisco.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.repository.Util
{
    public class CompetenciaRepository : RepositoryBase<Competencia>, ICompetenciaRepository
    {
        public CompetenciaRepository(Context db) : base(db)
        {
        }

        public Competencia GetComptenciaByAnoAndMes(int ano, int mes)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Mes == mes && competencia.Ano == ano
                         select competencia).FirstOrDefault();
            return query;
        }

        public Task<Competencia> GetComptenciaByAnoAndMesAsync(int ano, int mes)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Mes == mes && competencia.Ano == ano
                         select competencia).FirstOrDefaultAsync();
            return query;
        }

        public List<Competencia> GetComptenciasByAno(int ano)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Ano == ano
                         select competencia).ToList();
            return query;
        }

        public Task<List<Competencia>> GetComptenciasByAnoAsync(int ano)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Ano == ano
                         select competencia).ToListAsync();
            return query;
        }

        public List<Competencia> GetComptenciasByMes(int mes)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Mes == mes
                         select competencia).ToList();
            return query;
        }

        public Task<List<Competencia>> GetComptenciasByMesAsync(int mes)
        {
            var query = (from competencia in _db.Competencias
                         where competencia.Mes == mes
                         select competencia).ToListAsync();
            return query;
        }
    }
}
