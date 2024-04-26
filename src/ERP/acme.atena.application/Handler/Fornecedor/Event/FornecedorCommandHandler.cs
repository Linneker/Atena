using acme.atena.application.Handler.Fornecedores.Commands.Request;
using acme.atena.core.Mediador;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Fornecedores.Event
{
    public class FornecedorCommandHandler :
        IRequestHandler<FornecedorCommand, bool>
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;
        public FornecedorCommandHandler(IFornecedorRepository fornecedorRepository,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(FornecedorCommand request, CancellationToken cancellationToken)
        {
            var validacao = request.EhValido();
            if (validacao.IsValid)
            {
                var fornecedor = await _fornecedorRepository.GetByIdAsync(request.AggregateId);
                if(fornecedor is null)
                {
                    var novoFornecedor = _mapper.Map<Fornecedor>(request);

                }
            }
            else
            {
            }
            return false;
        }
    }
}
