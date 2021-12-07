using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Util
{
    public class EnderecoService : ServiceBase<Endereco>, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository repositoryBase) : base(repositoryBase)
        {
            _enderecoRepository = repositoryBase;
        }
    }
}
