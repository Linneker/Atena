using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Security
{
    public interface IAutorizacaoApiApplication: IApplicationBase<AutorizacaoApi>
    {
        AutorizacaoApi GetAutorizacaoByAccessKey(string accessKey);
        Task<AutorizacaoApi> GetAutorizacaoByAccessKeyAsync(string accessKey);

    }
}
