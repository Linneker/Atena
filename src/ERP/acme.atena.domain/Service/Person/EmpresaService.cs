using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Person
{
    public class EmpresaService : ServiceBase<Empresa>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaService(IEmpresaRepository repositoryBase) : base(repositoryBase)
        {
            _empresaRepository = repositoryBase;
        }

        public Empresa GetEmpresaByCnpj(string cnpj)
        {
            return _empresaRepository.GetEmpresaByCnpj(cnpj);
        }

        public Task<Empresa> GetEmpresaByCnpjAsync(string cnpj)
        {
            return _empresaRepository.GetEmpresaByCnpjAsync(cnpj); ;
        }

        public Empresa GetEmpresaByNome(string nome)
        {
            return _empresaRepository.GetEmpresaByNome(nome); ;
        }

        public Task<Empresa> GetEmpresaByNomeAsync(string nome)
        {
            return _empresaRepository.GetEmpresaByNomeAsync(nome); ;
        }
    }
}
