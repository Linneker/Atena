using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Util
{
    public class EnderecoApplication: ApplicationBase<Endereco>,IEnderecoApplication
    {
        private readonly IEnderecoService _enderecoService;

        public EnderecoApplication(IEnderecoService enderecoService):base(enderecoService)
        {
            _enderecoService = enderecoService;
        }
    }
}
