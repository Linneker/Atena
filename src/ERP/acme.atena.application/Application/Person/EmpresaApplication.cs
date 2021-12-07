using acme.atena.application.Interface.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Person
{
    public class EmpresaApplication : ApplicationBase<Empresa>, IEmpresaApplication
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaApplication(IEmpresaService empresaService):base(empresaService)
        {
            _empresaService = empresaService;
        }

        public Empresa GetEmpresaByCnpj(string cnpj)
        {
            return _empresaService.GetEmpresaByCnpj(cnpj);
        }

        public Task<Empresa> GetEmpresaByCnpjAsync(string cnpj)
        {
            return _empresaService.GetEmpresaByCnpjAsync(cnpj);
        }

        public Empresa GetEmpresaByNome(string nome)
        {
            return _empresaService.GetEmpresaByNome(nome);
        }

        public Task<Empresa> GetEmpresaByNomeAsync(string nome)
        {
            return _empresaService.GetEmpresaByNomeAsync(nome);
        }
    }
}
