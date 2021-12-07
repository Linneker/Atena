using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Util
{
    public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(Context db) : base(db)
        {
        }

        public Endereco GetEnderecoByCep(string cep)
        {
            Endereco endereco = (from end in _db.Enderecos
                                 where end.Cep.Equals(cep)
                                 select end).AsNoTracking().FirstOrDefault();

            return endereco;
        }

        public Task<Endereco> GetEnderecoByCepAsync(string cep)
        {
            Task<Endereco> endereco = (from end in _db.Enderecos
                                 where end.Cep.Equals(cep)
                                 select end).AsNoTracking().FirstOrDefaultAsync();

            return endereco;
        }
    }
}
