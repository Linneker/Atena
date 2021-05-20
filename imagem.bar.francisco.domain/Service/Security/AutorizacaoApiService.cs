using imagem.bar.francisco.domain.DTO.Seguranca;
using imagem.bar.francisco.domain.Interface.Repository.Security;
using imagem.bar.francisco.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace imagem.bar.francisco.domain.Service.Security
{
    public class AutorizacaoApiService : ServiceBase<AutorizacaoApi>, IAutorizacaoApiService
    {
        private readonly IAutorizacaoApiRepository _autorizacaoApiRepository;

        public AutorizacaoApiService(IAutorizacaoApiRepository autorizacaoApiRepository):base(autorizacaoApiRepository)
        {
            _autorizacaoApiRepository = autorizacaoApiRepository;
        }

        public AutorizacaoApi GetAutorizacaoByAccessKey(string accessKey)
        {
            return _autorizacaoApiRepository.GetAutorizacaoByAccessKey(accessKey);
        }

        public Task<AutorizacaoApi> GetAutorizacaoByAccessKeyAsync(string accessKey)
        {
            return _autorizacaoApiRepository.GetAutorizacaoByAccessKeyAsync(accessKey);
        }
    }
}
