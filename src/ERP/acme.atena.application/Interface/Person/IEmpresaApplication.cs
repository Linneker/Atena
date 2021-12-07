using acme.atena.domain.DTO.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Person
{
    public interface IEmpresaApplication : IApplicationBase<Empresa>
    {
        Empresa GetEmpresaByCnpj(string cnpj);
        Empresa GetEmpresaByNome(string nome);

        Task<Empresa> GetEmpresaByNomeAsync(string nome);
        Task<Empresa> GetEmpresaByCnpjAsync(string cnpj);
    }
}
