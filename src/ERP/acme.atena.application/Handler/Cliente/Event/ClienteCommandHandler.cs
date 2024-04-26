using acme.atena.application.Handler.Cliente.Commands.Input;
using acme.atena.core.Mediador;
using acme.atena.domain.Interface.Repository.Person;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Cliente.Event
{
    public class ClienteCommandHandler :
        IRequestHandler<CadastroClienteCommand, bool>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        public ClienteCommandHandler(
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CadastroClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = _mapper.Map<domain.DTO.Person.Cliente>(request);
            await _clienteRepository.AddAsync(cliente);
            
            return false;
        }
    }
}
