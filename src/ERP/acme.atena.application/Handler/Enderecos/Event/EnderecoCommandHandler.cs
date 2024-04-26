using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.application.Handler.Enderecos.Query;
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
    public class EnderecoCommandHandler :
        IRequestHandler<EnderecoCommand, Guid>
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public EnderecoCommandHandler(IEnderecoRepository enderecoRepository, IMapper mapper)
        {
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(EnderecoCommand request, CancellationToken cancellationToken)
        {
            Endereco endereco = _mapper.Map<Endereco>(request);
            await _enderecoRepository.AddAsync(endereco);
            bool salvo = await _enderecoRepository.UnitOfWork.CommitAsync();
            if (salvo)
                return endereco.Id;
            else
                throw new Exception("Endereço não cadastrado");
        }
    }
}
