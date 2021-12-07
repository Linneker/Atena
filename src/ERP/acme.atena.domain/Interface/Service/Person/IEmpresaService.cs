using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Person
{
    public interface IEmpresaService : IServiceBase<Empresa>
    {
        Empresa GetEmpresaByCnpj(string cnpj);
        Empresa GetEmpresaByNome(string nome);

        Task<Empresa> GetEmpresaByNomeAsync(string nome);
        Task<Empresa> GetEmpresaByCnpjAsync(string cnpj);
    }
}
