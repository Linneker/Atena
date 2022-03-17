using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Util
{
    internal class RequestDefaultApplication
    {
        public async Task<TEntity> RequestCompleta<TEntity, TRequest>(TRequest request, ConfiguracaoServico configuracao)
          where TEntity : class
          where TRequest : class
        {
            var client = new HttpClient();
            string url = $"{configuracao.Url}{configuracao.EndpointDefault}{configuracao.EndpointComplement}";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (configuracao.IsSecurity)
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuracao.AccessKey}");
            string jsonRequest = "";
            Task<HttpResponseMessage> response = null;
            HttpContent httpContent = null;
            switch (configuracao.TypeService)
            {
                case EnumTypeService.POST:
                    jsonRequest = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    response = client.PostAsync(url, httpContent);
                    break;
                case EnumTypeService.PUT:
                    jsonRequest = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    response = client.PutAsync(url, httpContent);
                    break;
                case EnumTypeService.DELETE:
                    response = client.DeleteAsync($"{url}/{request}");
                    break;
                case EnumTypeService.GET:
                    response = client.GetAsync($"{url}/{request}");
                    break;
                case EnumTypeService.PATH:
                    jsonRequest = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    response = client.PatchAsync(configuracao.Url, httpContent);
                    break;
            }
            HttpResponseMessage retorno = response.Result;
            string retornoMesnagem = "";
            if (retorno.IsSuccessStatusCode)
            {
                retorno.EnsureSuccessStatusCode();
                retornoMesnagem = await retorno.Content.ReadAsStringAsync();
            }

            try
            {
                TEntity json = JsonConvert.DeserializeObject<TEntity>(retornoMesnagem);
                return json;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
