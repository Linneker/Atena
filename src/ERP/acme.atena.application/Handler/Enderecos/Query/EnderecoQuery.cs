using acme.atena.application.Application.Util;
using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.application.Handler.Enderecos.Commands.Response;
using acme.atena.core.Mediador;
using acme.atena.domain.Interface.Repository.Util;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Enderecos.Query
{
    public class EnderecoQuery : IEnderecoQuery
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly RecuperarEnderecoViaCep _recuperarEnderecoViaCep;
        private readonly IMapper _mapper;

        public EnderecoQuery(IEnderecoRepository enderecoRepository, 
            IMediatorHandler mediatorHandler, 
            RecuperarEnderecoViaCep recuperarEnderecoViaCep, IMapper mapper)
        {
            _enderecoRepository = enderecoRepository;
            _mediatorHandler = mediatorHandler;
            _recuperarEnderecoViaCep = recuperarEnderecoViaCep;
            _mapper = mapper;
        }

        public async Task<EnderecoCommandQuery> ObterPeloCep(EnderecoCommandQuery enderecoQuery)
        {
            var endereco = await _enderecoRepository.GetEnderecoByCepAsync(enderecoQuery.Cep);
            if (endereco is null)
            {
                EnderecoCommand enderecoCommand = null;
                var enderecoViaCep  = await _recuperarEnderecoViaCep.ObterEndereco(enderecoQuery.Cep);
                if (enderecoViaCep is null)
                {
                    enderecoCommand = new EnderecoCommand(enderecoQuery.Cep, enderecoQuery.Pais, enderecoQuery.Estado, enderecoQuery.Cidade, enderecoQuery.Bairro, enderecoQuery.Rua);
                }
                else
                    enderecoCommand = new EnderecoCommand(enderecoViaCep.Cep, enderecoQuery.Pais, enderecoViaCep.Uf, enderecoViaCep.Localidade, enderecoViaCep.Bairro, enderecoViaCep.Logradouro);

                await _mediatorHandler.EnviarComandoGuid(enderecoCommand);
                
            }
            endereco = await _enderecoRepository.GetEnderecoByCepAsync(enderecoQuery.Cep);
            var enderecoRetornoQuery = _mapper.Map<EnderecoCommandQuery>(endereco);
            return enderecoRetornoQuery;
        }
    }
}
