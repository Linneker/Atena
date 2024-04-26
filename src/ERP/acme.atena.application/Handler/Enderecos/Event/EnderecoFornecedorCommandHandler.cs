using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.application.Handler.Enderecos.Commands.Response;
using acme.atena.core.Mediador;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository.Util;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Event
{
    public class EnderecoFornecedorCommandHandler :
        IRequestHandler<EnderecoFornecedorCommand, bool>
    {

        private readonly IEnderecoFornecedorRepository _enderecoFornecedorRepository;
        private readonly IMapper _mapper;
        public EnderecoFornecedorCommandHandler(IEnderecoFornecedorRepository enderecoFornecedorRepository,
            IMapper mapper)
        {
            _enderecoFornecedorRepository = enderecoFornecedorRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EnderecoFornecedorCommand request, CancellationToken cancellationToken)
        {
            var enderecoFornecedor = _mapper.Map<EnderecoFornecedor>(request);
            await _enderecoFornecedorRepository.AddAsync(enderecoFornecedor);
            var cadastrado = await _enderecoFornecedorRepository.UnitOfWork.CommitAsync();
            return cadastrado;
        }
    }
}
