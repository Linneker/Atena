using acme.atena.application.Handler.Enderecos.Commands.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Query
{
    public interface IEnderecoQuery
    {
        Task<EnderecoCommandQuery> ObterPeloCep(EnderecoCommandQuery cep);
    }
}
