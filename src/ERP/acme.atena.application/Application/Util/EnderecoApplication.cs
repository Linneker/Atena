﻿using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Service.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Util
{
    public class EnderecoApplication : ServiceBase<Endereco>, IEnderecoApplication
    {
        private readonly IEnderecoService _enderecoService;

        public EnderecoApplication(IEnderecoService enderecoService) : base(enderecoService)
        {
            _enderecoService = enderecoService;
        }

        public Endereco GetEnderecoByCep(string cep)
        {
            Endereco endereco = _enderecoService.GetEnderecoByCep(cep);
            if (endereco is null)
            {
                var client = new RestClient($"https://viacep.com.br/ws/{cep}/json");
                var request = new RestRequest("", Method.GET);
                var queryResult = client.Execute<EnderecoViaCep>(request).Data;
                endereco = new Endereco()
                {
                    Pais = "",
                    Estado = queryResult.Uf,
                    Cidade = queryResult.Localidade,
                    Bairro = queryResult.Bairro,
                    Cep = queryResult.Cep,
                    Rua = queryResult.Logradouro
                };
            }
            return endereco;
        }

        public async Task<Endereco> GetEnderecoByCepAsync(string cep)
        {
            Endereco endereco = await _enderecoService.GetEnderecoByCepAsync(cep);
            if (endereco is null)
            {
                RequestDefaultApplication requestDefault = new RequestDefaultApplication();

                var queryResult = await requestDefault.RequestCompleta<EnderecoViaCep, string>("", new ConfiguracaoServico($"https://viacep.com.br/ws/", cep, "/json", false, domain.DTO.Enum.EnumTypeService.GET, "", ""));
                endereco = new Endereco()
                {
                    Pais = "",
                    Estado = queryResult.Uf,
                    Cidade = queryResult.Localidade,
                    Bairro = queryResult.Bairro,
                    Cep = queryResult.Cep.Replace("-",""),
                    Rua = queryResult.Logradouro
                };
            }
            return endereco;
        }
    }
}
