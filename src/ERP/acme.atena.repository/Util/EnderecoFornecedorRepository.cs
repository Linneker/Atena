using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Util
{
    public class EnderecoFornecedorRepository : RepositoryBase<EnderecoFornecedor>, IEnderecoFornecedorRepository
    {
        public EnderecoFornecedorRepository(Context db) : base(db)
        {
        }

        public async Task<EnderecoFornecedor> ObterPeloCepNumero(string cep, string numero)
        => await _db.EnderecoFornecedor.Include(t => t.Endereco).Where(t => t.Endereco.Cep == cep && t.Numero == numero).FirstOrDefaultAsync();
        
    }
}
