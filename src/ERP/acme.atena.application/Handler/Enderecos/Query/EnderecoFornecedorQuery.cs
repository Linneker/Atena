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
    public class EnderecoFornecedorQuery : IEnderecoFornecedorQuery
    {
        public readonly IEnderecoQuery _enderecoQuery;
        public readonly IEnderecoFornecedorRepository _enderecoFornecedorRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        public EnderecoFornecedorQuery(IEnderecoQuery enderecoQuery, 
            IEnderecoFornecedorRepository enderecoFornecedorRepository,
            IMediatorHandler mediatorHandler,
            IMapper mapper)
        {
            _enderecoQuery = enderecoQuery;
            _enderecoFornecedorRepository = enderecoFornecedorRepository;
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
        }

        public async Task<EnderecoFornecedorCommandQuery> ObterPeloCepENumero(EnderecoFornecedorCommandQuery enderecoQuery)
        {
            var enderecoFornecedor = await _enderecoFornecedorRepository.ObterPeloCepNumero(enderecoQuery.Endereco.Cep, enderecoQuery.Numero);
            if(enderecoFornecedor is null)
            {
                var endereco = await _enderecoQuery.ObterPeloCep(new EnderecoCommandQuery() { Cep= enderecoQuery.Endereco.Cep });
                enderecoQuery.EnderecoId = endereco.Id;
                bool registrado = await _mediatorHandler.EnviarComando(new EnderecoFornecedorCommand(enderecoQuery.Matriz, enderecoQuery.Numero, enderecoQuery.Complemento, enderecoQuery.Latitude, enderecoQuery.Longitude, enderecoQuery.EnderecoId));
                if (registrado)
                    throw (new Exception("Endereço não localizado"));
            }
            enderecoFornecedor = await _enderecoFornecedorRepository.ObterPeloCepNumero(enderecoQuery.Endereco.Cep, enderecoQuery.Numero);
            var enderecoFornecedorCommandQuery = _mapper.Map<EnderecoFornecedorCommandQuery>(enderecoFornecedor);
            return enderecoFornecedorCommandQuery;
        }
    }
}
