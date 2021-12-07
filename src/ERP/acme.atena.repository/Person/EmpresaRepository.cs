using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Person
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(Context db) : base(db)
        {
        }

        public Empresa GetEmpresaByCnpj(string cnpj)
        {
            Empresa empresa = (from emp in _db.Empresas
                       where emp.CpfCnpj.Equals(cnpj)
                       select emp).AsNoTracking().Include(t => t.EnderecoEmpresas).ThenInclude(t => t.Endereco).FirstOrDefault();
            return empresa;
        }
        public Task<Empresa> GetEmpresaByCnpjAsync(string cnpj)
        {
            Task<Empresa> empresa = (from emp in _db.Empresas
                               where emp.CpfCnpj.Equals(cnpj)
                               select emp).Include(t => t.EnderecoEmpresas).ThenInclude(t => t.Endereco).AsNoTracking().FirstOrDefaultAsync();
            return empresa;
        }

        public Empresa GetEmpresaByNome(string nome)
        {
            Empresa empresa = (from emp in _db.Empresas
                               where emp.Nome.Equals(nome)
                               select emp).Include(t => t.EnderecoEmpresas).ThenInclude(t => t.Endereco).AsNoTracking().FirstOrDefault();
            return empresa;
        }

        public Task<Empresa> GetEmpresaByNomeAsync(string nome)
        {
            Task<Empresa> empresa = (from emp in _db.Empresas
                                     where emp.Nome.Equals(nome)
                                     select emp).Include(t=>t.EnderecoEmpresas).ThenInclude(t=>t.Endereco).AsNoTracking().FirstOrDefaultAsync();
            return empresa;
        }
    }
}
