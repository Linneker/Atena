using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Repository.Util;
using acme.cashflow.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.Service.Util
{
    public class CompetenciaService : ServiceBase<Competencia>, ICompetenciaService
    {
        private readonly ICompetenciaRepository _competenciaRepository;

        public CompetenciaService(ICompetenciaRepository competenciaRepository):base(competenciaRepository)
        {
            _competenciaRepository = competenciaRepository;
        }

        public List<Competencia> GetCompetenciasOrderByDesc()
        {
            return _competenciaRepository.GetCompetenciasOrderByDesc();
        }

        public Competencia GetComptenciaByAnoAndMes(int ano, int mes)
        {
            return _competenciaRepository.GetComptenciaByAnoAndMes(ano, mes);
        }

        public Task<Competencia> GetComptenciaByAnoAndMesAsync(int ano, int mes)
        {
            return _competenciaRepository.GetComptenciaByAnoAndMesAsync(ano, mes); 
        }

        public List<Competencia> GetComptenciasByAno(int ano)
        {
            return _competenciaRepository.GetComptenciasByAno(ano);
        }

        public Task<List<Competencia>> GetComptenciasByAnoAsync(int ano)
        {
            return _competenciaRepository.GetComptenciasByAnoAsync(ano);
        }

        public List<Competencia> GetComptenciasByMes(int mes)
        {
            return _competenciaRepository.GetComptenciasByMes(mes);
        }

        public Task<List<Competencia>> GetComptenciasByMesAsync(int mes)
        {
            return _competenciaRepository.GetComptenciasByMesAsync(mes);
        }
    }
}
