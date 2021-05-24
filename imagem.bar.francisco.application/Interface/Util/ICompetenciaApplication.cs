using imagem.bar.francisco.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.application.Interface.Util
{
    public interface ICompetenciaApplication : IApplicationBase<Competencia>
    {
        Competencia GetComptenciaByAnoAndMes(int ano, int mes);
        Task<Competencia> GetComptenciaByAnoAndMesAsync(int ano, int mes);
        List<Competencia> GetComptenciasByAno(int ano);
        Task<List<Competencia>> GetComptenciasByAnoAsync(int ano);
        List<Competencia> GetComptenciasByMes(int mes);
        Task<List<Competencia>> GetComptenciasByMesAsync(int mes);
        Competencia CadastraCompetenciaSeNaoExistir();
        List<Competencia> GetCompetenciasOrderByDesc();
    }
}
