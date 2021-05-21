using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Service.Account
{
    public class DespesaService: ServiceBase<Despesa>, IDespesaService
    {
        private readonly IDespesaRepository _despesaRepository;

        public DespesaService(IDespesaRepository despesaRepository):base(despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public List<Despesa> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(int ano, int mes)
        {
            return _despesaRepository.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaRepository.GetDespesaByCompetenciaAnoAndCompetenciaMesOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesaByCompetenciaAnoOrderByMaiorValor(int ano)
        {
            return _despesaRepository.GetDespesaByCompetenciaAnoOrderByMaiorValor(ano);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(int ano)
        {
            return _despesaRepository.GetDespesaByCompetenciaAnoOrderByMaiorValorAsync(ano);
        }

        public List<Despesa> GetDespesaByCompetenciaId(Guid guid)
        {
            return _despesaRepository.GetDespesaByCompetenciaId(guid);
        }

        public List<Despesa> GetDespesaByCompetenciaMesOrderByMaiorValor(int mes)
        {
            return _despesaRepository.GetDespesaByCompetenciaMesOrderByMaiorValor(mes);
        }

        public Task<List<Despesa>> GetDespesaByCompetenciaMesOrderByMaiorValorAsync(int mes)
        {
            return _despesaRepository.GetDespesaByCompetenciaMesOrderByMaiorValorAsync(mes);
        }

        public List<Despesa> GetDespesaByPeriodoCompetenciaOrderByMaiorValor(int ano, int mes)
        {
            return _despesaRepository.GetDespesaByPeriodoCompetenciaOrderByMaiorValor(ano, mes);
        }

        public Task<List<Despesa>> GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(int ano, int mes)
        {
            return _despesaRepository.GetDespesaByPeriodoCompetenciaOrderByMaiorValorAsync(ano, mes);
        }

        public List<Despesa> GetDespesaOrderByMaiorValor()
        {
            return _despesaRepository.GetDespesaOrderByMaiorValor();
        }

        public Task<List<Despesa>> GetDespesaOrderByMaiorValorAsync()
        {
            return _despesaRepository.GetDespesaOrderByMaiorValorAsync();
        }
    }
}
