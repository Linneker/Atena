using acme.cashflow.application.Interface.Util;
using acme.cashflow.domain.DTO.Enum;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.application.Application.Util
{
    public class CompetenciaApplication : ApplicationBase<Competencia>, ICompetenciaApplication
    {
        private readonly ICompetenciaService _competenciaService;

        public CompetenciaApplication(ICompetenciaService competenciaService) : base(competenciaService)
        {
            _competenciaService = competenciaService;
        }

        public Competencia CadastraCompetenciaSeNaoExistir()
        {
            Competencia competencia = GetComptenciaByAnoAndMes(DateTime.Now.Year, DateTime.Now.Month);
            if (competencia is null)
            {
                competencia = new Competencia(DateTime.Now.Year, DateTime.Now.Month);
                this.Add(competencia, "COMPETÊNCIA");
            }
            return competencia;
        }

        public List<Competencia> GetCompetenciasOrderByDesc()
        {
          CadastraCompetenciaSeNaoExistir();
          return _competenciaService.GetCompetenciasOrderByDesc();
        }

        public Competencia GetComptenciaByAnoAndMes(int ano, int mes)
        {
            return _competenciaService.GetComptenciaByAnoAndMes(ano, mes);
        }

        public Task<Competencia> GetComptenciaByAnoAndMesAsync(int ano, int mes)
        {
            return _competenciaService.GetComptenciaByAnoAndMesAsync(ano, mes);
        }

        public List<Competencia> GetComptenciasByAno(int ano)
        {
            return _competenciaService.GetComptenciasByAno(ano); ;
        }

        public Task<List<Competencia>> GetComptenciasByAnoAsync(int ano)
        {
            return _competenciaService.GetComptenciasByAnoAsync(ano); ;
        }

        public List<Competencia> GetComptenciasByMes(int mes)
        {
            return _competenciaService.GetComptenciasByMes(mes);
        }

        public Task<List<Competencia>> GetComptenciasByMesAsync(int mes)
        {
            return _competenciaService.GetComptenciasByMesAsync(mes);
        }
    }
}
