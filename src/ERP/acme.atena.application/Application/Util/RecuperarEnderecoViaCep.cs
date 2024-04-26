using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Util
{
    public class RecuperarEnderecoViaCep
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public RecuperarEnderecoViaCep(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        public async Task<EnderecoViaCep> ObterEndereco(string cep)
        {
            EnderecoViaCep endereco = new EnderecoViaCep();
            var httpClient = _httpClientFactory.CreateClient("ViaCep");
            var httpResponseMessage = await httpClient.GetAsync($"{cep.Replace(".", "").Replace("-", "")}/json/");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                endereco = await JsonSerializer.DeserializeAsync<EnderecoViaCep>(contentStream);
            }
            else
                throw (new Exception("Não é possivel recuperar endereço"));
            return endereco;
        }
    }
}
